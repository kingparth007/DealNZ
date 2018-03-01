using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        string Domain = "";
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
                    return RedirectToAction("Index", "Register_Login");
                }
                else
                {
                    ViewBag.Regisert_Login = "Please Try to Login For New Link";
                    return RedirectToAction("Index", "Register_Login");
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

                    string body = string.Empty;
                    //  using streamreader for reading my htmltemplate
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemp/ResetPasswordTemplate.html")))
                    {

                        body = reader.ReadToEnd();

                    }
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        Domain = "http://localhost:20629";
                    }
                    else
                    {
                        Domain = ConfigurationManager.AppSettings["Domain"];
                    }
                    string URL = Domain + "/Register_Login/Reset/" + ForgotPasswordUser.UserVerificationCode;
                    body = body.Replace("{LinkUrl}", URL); //replacing the required things  

                    UserProfileService.UserMail(body, "For Reset Password", checkemail.Email);
                    ViewBag.ForgotPassError = "Check Your Mail for Reset Link";
                    return View();
                }
                ViewBag.ForgotPassError = "EmailId Does not Exists";
            }
            return View(ForgotPass);
        }
    }
}