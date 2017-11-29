using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;

namespace DealsNZ.Controllers.UserController
{
    public class ActivationController : Controller
    {
        // GET: Activation

        IUserProfile UserProfileService;
        //IUserType up = new UserTypeService(new DealsDB());
        IUserVerification UserVerificationService = new UserVerificationService(new DealsDB());
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Activate(string gui)
        {

            //string guid = (string)this.RouteData.Values["guid"];

            if (RouteData.Values["id"] != null)
            {

                string guid = RouteData.Values["id"].ToString();
                if (UserVerificationService.UserActivate(guid) == true)
                {
                    ViewBag.Regisert_Login = "Verified Sucessfully Process to login";
                    return Redirect(Url.Action("Index", "Register_Login"));
                }
            }

            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(AccountModels.ForgotPass ForgotPass)
        {
            if (ModelState.IsValid)
            {
                UserProfileService = new UserProfileServices(new DealsDB());
                var checkemail = UserProfileService.GetUserByEmail(ForgotPass.ForgotPassEmail);
                if (checkemail != null)
                {
                    Guid guid = Guid.NewGuid();
                    UserVerification ForgotPasswordUser = new UserVerification()
                    {
                        Purpose = "ResetPass",
                        Userid = checkemail.UserId,
                        UserVerificationCode = guid,
                        AddedOn = System.DateTime.Now.Date
                    };

                    UserVerificationService.Insert(ForgotPasswordUser);
                    string URL = "http://localhost:20629/Register_Login/Reset/" + ForgotPasswordUser.UserVerificationCode;
                    UserProfileService.UserMail(URL, "For Reset Password", checkemail.Name, checkemail.Email);
                    ViewBag.ForgotPassError = "Check Your Mail for Reset Link";
                    return View();
                }
                ViewBag.ForgotPassError = "Email Does not Exists";

            }
            return View(ForgotPass);
        }
    }
}