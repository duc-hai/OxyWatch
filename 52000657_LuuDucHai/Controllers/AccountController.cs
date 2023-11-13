using _52000657_LuuDucHai.Models;
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BotDetect.Web.UI;
using BotDetect.Web;
using System.Web.Razor.Parser.SyntaxTree;
using BotDetect.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace _52000657_LuuDucHai.Controllers
{
    public class AccountController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        // GET: Account
        public ActionResult Index()
        {
            login();
            return View();
        }

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login (string username, string password)
        {
            //Kiểm tra hợp lệ của dữ liệu đầu vào
            if (username == null || password == null || username.Trim() == "" || password.Trim() == "")
            {
                errAccount(username, password, "Vui lòng nhập đủ thông tin");
                return View();
            }

            if (password.Length < 6)
            {
                errAccount(username, password, "Mật khẩu phải có ít nhất 6 kí tự");
                return View();
            }

            //Kiểm tra username và mật khẩu trong db
            var account = (from t in db.accounts
                           where t.username == username.ToLower().Trim() && t.role == 1
                           select t).FirstOrDefault();
            //Username không tồn tại hoặc sai mật khẩu
            if (account == null || BCrypt.Net.BCrypt.Verify(password.Trim(), account.password) == false)
            {
                errAccount(username, password, "Tài khoản hoặc mật khẩu không hợp lệ"); //Không nên trả ra lỗi quá chi tiết vì có thể bị hack
                return View();
            }
            //Đăng nhập thành công
            Session["ID"] = account.id;
            Session["username"] = username;

            //Tạo alert message
            setAlert("Đăng nhập thành công", "success");
            return RedirectToAction("Index", "Default");
        }

        //Hiển thị trang đăng ký
        public ActionResult signup ()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Mã captcha không hợp lệ")]
        public ActionResult signup (string username, string password, string confirmPassword) //Thực hiện đăng ký
        {
            //Kiểm tra captcha 
            if (!ModelState.IsValid)
            {
                errAccount(username, password, "Mã captcha không hợp lệ");
                return View();
            }

            //Kiểm tra hợp lệ của dữ liệu đầu vào
            if (username == null || password == null || confirmPassword == null || username.Trim() == "" || password.Trim() == "" || confirmPassword.Trim() == "")
            {
                errAccount(username, password, "Vui lòng nhập đủ thông tin");
                return View();
            }

            if (password.Length < 6)
            {
                errAccount(username, password, "Mật khẩu phải có ít nhất 6 kí tự");
                return View();
            }

            if (password.Trim() != confirmPassword.Trim())
            {
                errAccount(username, password, "Mật khẩu xác nhận không đúng");
                return View();
            }

            //Kiểm tra xem username có tồn tại hay chưa
            var user = from t in db.accounts
                       where t.username == username.Trim()
                       select t;
            if (user.Count() != 0)
            {
                errAccount(username, password, "Username đã tồn tại trong hệ thống");
                return View();
            }

            //Để tăng tính bảo mật, cần hash password trước khi đưa vào hệ thống để hacker có hack đc vào db cũng không thể lấy được mật khẩu 
            //Cách cài đặt Bcrypt package trong Visual studio:
            //Tool -> NuGet Package Manager -> Package Manager Console
            //Nhập: Install-Package BCrypt.Net-Next
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password.Trim());

            var account = new account();
            account.username = username.Trim();
            account.password = hashPassword;
            account.role = 1;
            db.accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("login", "Account");
        }

        public void setAlert(string message, string type)
        {
            TempData["alert"] = message;
            TempData["type"] = "alert-" + type;
        }

        public void errAccount (string username, string pwd, string msg)
        {
            TempData["username"] = username; //Trả lại dữ liệu cũ cho người dùng
            TempData["password"] = pwd;
            TempData["error"] = msg; //Thông báo lỗi
        }

        public ActionResult logOut ()
        {
            Session.Remove("username");
            Session.Remove("ID");
            // Dùng để đăng xuất form của người dùng, việc xóa dữ liệu khỏi cookie sẽ tránh tự đăng nhập lại mặc dù đã bấm đăng xuất
            FormsAuthentication.SignOut();
            setAlert("Đăng xuất thành công", "success");
            return RedirectToAction("Index", "Default");
        }

        private Uri RedirectUri
        {
            //Tạo 1 URL sẽ callback sau khi người dùng xác nhận login
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult loginFacebook()
        {
            //Vì 1 số chính sách của facebook nên hiện tại chưa thể đăng nhập với tài khoản nào khác ngoài tài khoản fb admin
            //Nên để thuận tiện cho việc test, vui lòng sử dụng account facebook dưới đây để login
            //oceanvlogg@gmail.com
            //oceanvlogg123

            var fb = new FacebookClient();
            //Đường dẫn login
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1385588488897909", //Lấy từ developers.facebook
                client_secret = "50c5515111653a64fe8c963188b015b6", //Lấy từ developers.facebook
                redirect_uri = RedirectUri.AbsoluteUri, //Callback trả về sau khi login 
                response_type = "code", //Loại phản hồi là code
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        //Sau khi người dùng đã login, dữ liệu của người dùng sẽ về hàm callback này
        public ActionResult FacebookCallBack (string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "1385588488897909",
                client_secret = "50c5515111653a64fe8c963188b015b6",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Lấy thông tin của user
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string id = me.id;
                //Ngoài ra còn có thể lấy thêm firstName, middleName, lastName cũng như email, nhưng trong phạm vi login này
                //chỉ lấy ra id

                var username = "FB" + id; //username lưu trong db có định dạng FB + <id user>
                //Kiểm tra xem user đã tồn tại trong db chưa, nếu chưa thì tạo mới, nếu rồi thì login
                var user = from t in db.accounts
                           where t.username == username
                           select t;
                if (user.Count() == 0)
                {
                    //User chưa tồn tại, lưu thông tin user vào db
                    var account = new account();
                    account.username = username;
                    account.role = 1;
                    db.accounts.Add(account);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Default");
            }
            return RedirectToAction("login", "Account");
        }
    }
}