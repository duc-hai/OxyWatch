using _52000657_LuuDucHai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ImageManipulation;

namespace _52000657_LuuDucHai.Controllers
{
    public class CartController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        private const string cartSession = "cartSession";
        // GET: Cart
        public ActionResult Index()
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            ViewBag.metaDetail = "san-pham";
            var cart = Session[cartSession]; //Lấy ra danh sách cart từ session
            var list = new List<CartItem>();
            if (cart != null) //Nếu cart rỗng thì khởi tạo danh sách mới
            {
                list = (List<CartItem>)cart;
                Session["cartNumber"] = list.Count();
            }
            else
            {
                Session["cartNumber"] = 0;
            }
            return View(list);
        }
        
        public ActionResult addItem (int productId, int quantity)
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            //Lấy ra product với id tương ứng
            var product = (from t in db.products
                           where t.hide == true && t.id == productId
                           select t).FirstOrDefault(); 

            if (product == null)
            {
                throw new Exception("Thông tin sản phẩm không đúng, vui lòng thử lại");
            }
         
            var cart = Session[cartSession];
            //Giỏ hàng đã có sản phẩm
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                //Tìm xem trong giỏ hàng đã có sản phẩm với id đó chưa
                //Nếu rồi thì cộng quantity vào cho sản phẩm đó
                if (list.Exists(x => x.product.id == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.product.id == productId)
                        {
                            item.quantity += quantity;
                        }
                    }
                }

                //Nếu chưa thì thêm mới sản phẩm
                else
                {
                    var item = new CartItem();
                    item.product = product;
                    item.quantity = quantity;
                    list.Add(item);
                }
                Session[cartSession] = list;
            }
            //Giỏ hàng chưa có sản phẩm thì thêm sản phẩm
            else
            {
                var item = new CartItem();
                item.product = product;
                item.quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[cartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult updateCart (string newCart)
        {
            try
            {
                if (checkLogin() == false)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Please login"
                    });
                }
                var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(newCart); //Parse newCart từ dạng chuỗi sang dạng JSON
                var sessionCart = (List<CartItem>)Session[cartSession]; //Lấy ra danh sách cart trong session

                foreach (var item in sessionCart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.product.id == item.product.id); //Lấy ra phần tử có id trùng với id của product
                    if (jsonItem != null)
                    {
                        item.quantity = jsonItem.quantity; //Sau khi lấy ra thì set lại quantity mới
                    }
                }
                Session[cartSession] = sessionCart; //Set lại giá trị mới của list cart vào session
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false,
                    message = e.Message
                });
            }
        }

        public JsonResult deleteAll ()
        {
            if (checkLogin() == false)
            {
                return Json(new
                {
                    status = false,
                    message = "Please login"
                });
            }
            Session[cartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult delete (int id)
        {
            try
            {
                if (checkLogin() == false)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Please login"
                    });
                }
                var sessionCart = (List<CartItem>)Session[cartSession];
                sessionCart.RemoveAll(x => x.product.id == id);
                Session[cartSession] = sessionCart;
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false,
                    message = e.Message
                });
            }
        }

        [HttpGet]
        public ActionResult orderProduct()
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            var cart = Session[cartSession]; //Lấy ra danh sách cart từ session
            var list = new List<CartItem>();
            if (cart != null) //Nếu cart rỗng thì khởi tạo danh sách mới
            {
                list = (List<CartItem>)cart;
            }
            else
            {
                //Hiển thị thông báo rỗng và trả về
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult orderProduct(string fullName, string email, string phone, string address, string locationProvince, string locationDistrict)
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            TempData["fullName"] = fullName;
            TempData["email"] = email;
            TempData["phone"] = phone;
            TempData["address"] = address;
            TempData["locationProvince"] = locationProvince;
            TempData["locationDistrict"] = locationDistrict;
            return Redirect("phuong-thuc-thanh-toan"); //Show giao diện trang thanh toán
        }

        [HttpGet]
        public ActionResult payment()
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            var cart = Session[cartSession]; //Lấy ra danh sách cart từ session
            var list = new List<CartItem>();
            if (cart != null) //Nếu cart rỗng thì khởi tạo danh sách mới
            {
                list = (List<CartItem>)cart;
            }
            else
            {
                //Hiển thị thông báo rỗng và trả về
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult addOrder(string fullName, string email, string phone, string address, string locationProvince, string locationDistrict, string payment_method)
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            //Thêm mới đơn hàng
            var order = new order(); 
            order.customerId = (int)Session["ID"];
            order.fullName = fullName;
            order.email = email;
            order.phone = phone;
            order.address = address;
            order.locationDistrict = locationDistrict;
            order.locationProvince = locationProvince;
            order.payment_method = payment_method;
            order.status = "Chờ xác nhận";
            order.datebegin = Convert.ToDateTime(DateTime.Now.ToString()); //Get current datetime
            db.orders.Add(order);
            db.SaveChanges();

            //Thêm mới chi tiết đơn hàng
            var listProduct = ""; //Danh sách sản phẩm (dùng để gửi mail cho khách hàng)
            var total = 0; //Tổng tiền
            var cart = Session[cartSession]; 
            var list = new List<CartItem>();
            list = (List<CartItem>)cart;
            foreach (var i in list)
            {
                var orderDetail = new orderDetail();
                orderDetail.quantity = i.quantity;
                orderDetail.productId = i.product.id;
                //Thêm 1 cột để lưu giá thay vì nếu lấy giá trong bảng product vì sẽ có trường hợp sau này giá sẽ thay đổi, vì thế cần lưu lại giá ngay tại thời điểm mà khách đặt mua
                orderDetail.price = i.product.newPrice;
                orderDetail.orderId = order.id; 
                total += i.quantity * (int)i.product.newPrice; //Tính tổng tiền đơn hàng

                var prod = (from t in db.products
                           where t.id == i.product.id
                           select t).FirstOrDefault();
                //Lưu danh sách sản phẩm để gửi email đơn hàng cho khách hàng
                listProduct += "<tr><td>"+ prod.name +"</td><td>" + i.quantity + "</td><td>" + Currency(i.product.newPrice) + "</td><td>" + Currency(i.quantity * i.product.newPrice) +"</td></tr>";
               
                db.orderDetails.Add(orderDetail);
                db.SaveChanges();
            }

            //Nội dung gửi mail đến khách hàng
            
            //Địa chỉ đơn hàng (bao gồm đường/phường + quận/huyện + tỉnh/tp
            string addr = order.address + ", " + order.locationDistrict + ", " + order.locationProvince;
            //Nội dung của email, ánh xạ tới file newOrder
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/newOrder.html"));
            //Trong file newOrder gồm các item chứa thông tin đơn hàng, vì thế cần đưa dữ liệu vào các item này (replace)
            content = content.Replace("{{id}}", order.id.ToString());
            content = content.Replace("{{name}}", order.fullName);
            content = content.Replace("{{phone}}", order.phone);
            content = content.Replace("{{address}}", addr);
            content = content.Replace("{{date}}", order.datebegin.ToString());
            content = content.Replace("{{total}}", Currency(total));
            content = content.Replace("{{list}}", listProduct);
            sendMail("Đơn hàng mới từ OxyWatch", content, order.email);
            setAlert("Đặt hàng thành công", "success"); //Trả về thông báo thành công
            Session[cartSession] = null; //Xóa dữ liệu giỏ hàng trong session
            return Redirect("/don-hang");
        }

        public void sendMail(string subject, string content, string toEmailAdd)
        {
            var fromEmailAdress = "autosendemail.watch@gmail.com"; //Email của người gửi
            //Mật khẩu ứng dụng tạo bởi google security, từ 5/2022 google ko còn hỗ trợ
            //đăng nhập chỉ với username và password đơn thuần mà phải tạo mật khẩu riêng (vào trang google security)
            var fromEmailPassword = "zpylbrtturqbnigf"; 
            var fromEmailDisplayName = "Oxy Watch Shop"; //Tên người gửi muốn hiển thị
            var toEmailAddress = toEmailAdd; //Địa chỉ email của người nhận (khách hàng)
            var smtpHost = "smtp.gmail.com"; //Server smtp
            var enabledSSL = true; //Bật chứng chỉ SSL
            var smtpPort = "587"; //Cổng smtp

            MailMessage message = new MailMessage(new MailAddress(fromEmailAdress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject; //Tiêu đề email
            message.IsBodyHtml = true; //Cho phép nội dung email là html
            message.Body = content; //Set nội dung email

            var client = new SmtpClient(); //Cấu hình
            client.UseDefaultCredentials = true;    
            client.Credentials = new NetworkCredential(fromEmailAdress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSSL;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message); //Gửi
        }

        public bool checkLogin ()
        {
            if (Session["username"] == null)
            {
                return false;
            }
            return true;
        }

        public void setAlert(string message, string type)
        {
            TempData["alert"] = message;
            TempData["type"] = "alert-" + type;
        }

        public string Currency(int? curr)
        {
            return String.Format("{0:#,#.}", curr) + "đ";
        }
    }
}