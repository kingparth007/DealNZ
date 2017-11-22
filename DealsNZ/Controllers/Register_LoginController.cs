using System.Web.Mvc;
using DealsNZ.Models;

namespace DealsNZ.Controllers
{
    public class Register_LoginController : Controller
    {
        // GET: Register_Login
        public ActionResult Index()
        {
            ViewBag.RegisterError = "Hii";
            ViewBag.LoginError = "Hello";
            return View();
        }
        public ActionResult RegisterUser()
        {

            return Redirect(Url.Action("Index", "Register_Login"));
        }
        [HttpPost]
        public ActionResult RegisterUser(AccountModels.Register Register)
        {

            if (ModelState.IsValid)
            {
                return Redirect(Url.Action("Index", "Register_Login"));
            }
            //return Redirect( Url.Action("Index", "Register_Login", Login));
            return View("Index", Register);
        }

        public ActionResult LoginUser()
        {
            return Redirect(Url.Action("Index", "Register_Login"));
        }
        [HttpPost]
        public ActionResult LoginUser(AccountModels.Login Login)
        {

            if (ModelState.IsValid)
            {

                return Redirect(Url.Action("Index", "Register_Login"));
            }
            //return Redirect( Url.Action("Index", "Register_Login", Login));
            return View("Index", Login);
        }
    }
}