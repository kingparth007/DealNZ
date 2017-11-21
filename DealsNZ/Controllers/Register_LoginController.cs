using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;

namespace DealsNZ.Controllers
{
    public class Register_LoginController : Controller
    {
        // GET: Register_Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Hello(UserProfile User)
        {

            if (ModelState.IsValid)
            {

            }
            return Redirect(Url.Action("Index", "Home"));
        }
    }
}