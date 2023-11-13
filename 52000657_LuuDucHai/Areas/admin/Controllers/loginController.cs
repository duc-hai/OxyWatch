using _52000657_LuuDucHai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class loginController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        // GET: admin/login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            //Kiểm tra hợp lệ của dữ liệu đầu vào 
            if (username == null || password == null || username.Trim() == "" || password.Trim() == "")
            {
                errorLogin(username, password, "Vui lòng nhập đủ thông tin");
                return View();
            }
            if (password.Length < 6)
            {
                errorLogin(username, password, "Mật khẩu phải có ít nhất 6 kí tự");
                return View();
            }
            //Kiểm tra tài khoản mật khẩu trong db
            var account = (from t in db.accounts
                           where t.username == username.ToLower().Trim() && t.role == 0
                           select t).FirstOrDefault();
            //Username không tồn tại hoặc sai mật khẩu
            if (account == null || BCrypt.Net.BCrypt.Verify(password.Trim(), account.password) == false)
            {
                errorLogin(username, password, "Tài khoản hoặc mật khẩu không hợp lệ");
                return View();
            }

            //Đăng nhập thành công
            TempData.Remove("username"); //Xóa các dữ liệu trong TempData nếu có
            TempData.Remove("password");
            TempData.Remove("error");
            Session["profileImg"] = account.images; //Truyền vào profile image
            Session["usernameAD"] = username;
            return RedirectToAction("Index", "Default", new { area = "admin" });
        }

        public void errorLogin (string username, string pwd, string msg)
        {
            TempData["username"] = username; //Trả lại dữ liệu cũ cho người dùng
            TempData["password"] = pwd;
            TempData["error"] = msg; //Thông báo lỗi
        }

        public ActionResult logOut ()
        {
            Session.Remove("usernameAD");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "login", new { area = "admin" });
        }
    }
}