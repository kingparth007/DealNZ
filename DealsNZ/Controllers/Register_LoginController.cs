using System;
using System.Web.Mvc;
using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Controllers
{
    public class Register_LoginController : Controller
    {

        IUserProfile UserProfileService = new UserProfileServices(new DealsDB());
        IUserType up = new UserTypeService(new DealsDB());

        KeyList KeyList = new KeyList();

        public ActionResult Index()
        {

            ViewBag.RegisterError = "";
            ViewBag.LoginError = "";
            ViewBag.ABC = new SelectList(up.Get(), "UserTypeId", "UserTypeName");
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
                var checkmail = UserProfileService.CheckEmail(Register.Email);
                if (checkmail == true)
                {

                    var checkinsert = UserProfileService.RegisterUser(Register);
                    if (checkinsert == true)
                    {
                        ViewBag.RegisterError = "Registration Sucessfully Done Please verify your account from your mail account";
                        Register.Name = Register.Password = Register.ConformPassword = Register.Email = string.Empty;
                        return View("Index", new AccountModels.Register());
                    }
                    ViewBag.RegisterError = "Somthing Wrong Try Again Please";
                    return View("Index", Register);
                }
                ViewBag.RegisterError = "User already exist by this email address";

                return View("Index", Register);


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
                var checkmail = UserProfileService.CheckEmail(Login.LogInEmail);
                if (checkmail == false)
                {
                    var loggeduser = UserProfileService.LoginDetail(Login);
                    if (loggeduser != null)
                    {
                        if (loggeduser.isContactVerified == true)
                        {
                            Session[KeyList.SessionKeys.UserEmail] = loggeduser.Email;
                            Session[KeyList.SessionKeys.UserType] = loggeduser.UserType1.UserTypeName;
                            Session[KeyList.SessionKeys.UserID] = loggeduser.UserId;
                            Logs GenerateLog = new Logs();

                            GenerateLog.CreateLog(loggeduser.UserId, KeyList.LogMessages.LoginMessage);

                            return Redirect(Url.Action("Index", "Home"));
                        }
                        ViewBag.LoginError = "Please Activate User for login. Check Your Mail";
                        return View("Index", Login);
                    }
                    ViewBag.LoginError = "Email and Password Does not match.Please try again";
                    return View("Index", Login);
                }

                ViewBag.LoginError = "User Does Not Exist";
                return View("Index", Login);
            }
            //return Redirect( Url.Action("Index", "Register_Login", Login));
            return View("Index", Login);
        }
        public ActionResult LoggedOut()
        {
            Logs GenerateLog = new Logs();
            GenerateLog.CreateLog(Convert.ToInt32(Session[KeyList.SessionKeys.UserID]), KeyList.LogMessages.LogOutMessage);
            Session[KeyList.SessionKeys.UserEmail] = Session[KeyList.SessionKeys.UserType] = null;
            Session[KeyList.SessionKeys.UserID] = null;
            Session.Abandon();
            return Redirect(Url.Action("Index", "Home"));
        }
        public ActionResult Reset()
        {
            if (RouteData.Values["id"] != null)
            {

                var GetUserForResetPass = UserProfileService.ForgotPassUser(RouteData.Values["id"].ToString());
                DateTime LinkDate = Convert.ToDateTime(GetUserForResetPass.AddedOn);
                if (LinkDate < LinkDate.AddMinutes(5))
                {
                    AccountModels.ResetPass ResetUser = new AccountModels.ResetPass();
                    ResetUser.ForgotUserID = GetUserForResetPass.Userid;
                    return View("ResetPassword", ResetUser);
                }

            }
            return View();
        }

        public ActionResult ResetPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(AccountModels.ResetPass ResetPassUser)
        {
            if (ModelState.IsValid)
            {
                UserProfile GetUserForResetPass = UserProfileService.GetByID(ResetPassUser.ForgotUserID);
                if (GetUserForResetPass != null)
                {

                    GetUserForResetPass.Password = UserProfileService.PasswordEncrypt(ResetPassUser.Password.ToString());
                    if (UserProfileService.UpdateUser(GetUserForResetPass) == true)
                    {
                        Logs GenerateLog = new Logs();

                        GenerateLog.CreateLog(GetUserForResetPass.UserId, KeyList.LogMessages.ResetPassword);
                        UserProfileService.RemoveLinkForResetPassword(GetUserForResetPass.UserId);
                        return Redirect(Url.Action("Index", "Register_Login"));
                    }
                }
                return Redirect(Url.Action("ForgotPassword", "Activation"));
            }
            return View(ResetPassUser);
        }
    }
}