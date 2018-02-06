using System;
using System.Web.Mvc;
using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using static DealsNZ.Models.AccountModels;

namespace DealsNZ.Controllers
{
    public class Register_LoginController : Controller
    {

        public static string url = "";
        IUserProfile UserProfileService = new UserProfileServices(new DealsDB());
        //  IUserType up = new UserTypeService(new DealsDB());

        //KeyList KeyList = new KeyList();

        public ActionResult Index()
        {
            url = Request.UrlReferrer.ToString();
            ViewBag.RegisterError = "";
            ViewBag.LoginError = "";
            if (Session[KeyList.SessionKeys.UserID] != null)
            {
                return Redirect(Url.Action("MyProfile", "Register_Login"));
            }
            //    ViewBag.ABC = new SelectList(up.GetAll(), "UserTypeId", "UserTypeName");
            return View();
        }
        public ActionResult RegisterUser()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return Redirect(Url.Action("Index", "Register_Login"));
            }
            return Redirect(Url.Action("MyProfile", "Register_Login"));
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
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return Redirect(Url.Action("Index", "Register_Login"));
            }
            return Redirect(Url.Action("MyProfile", "Register_Login"));
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
                            IUserWallet WalletService = new UserWalletServices(new DealsDB());
                            WalletService.ShowWalletAmount(Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString()));
                            Session[KeyList.SessionKeys.WalletCredit] = WalletService.ShowWalletAmount(Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString()));
                            WalletService.Dispose();
                            Logs GenerateLog = new Logs();

                            GenerateLog.CreateLog(loggeduser.UserId, KeyList.LogMessages.LoginMessage);

                            if (Session[KeyList.SessionKeys.UserType].ToString() == KeyList.Users.Admin)
                            {
                                return Redirect(Url.Action("Store", "Admin"));
                            }
                            if (Session[KeyList.SessionKeys.UserType].ToString() == KeyList.Users.Vendor)
                            {
                                return Redirect(Url.Action("Index", "Home"));
                            }
                            return Redirect(url);
                        }
                        else
                        {
                            UserProfileService.Veryfication(loggeduser.UserId, loggeduser.Email, loggeduser.Name);
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
            if (Session[KeyList.SessionKeys.UserID] != null)
            {
                Logs GenerateLog = new Logs();
                GenerateLog.CreateLog(Convert.ToInt32(Session[KeyList.SessionKeys.UserID]), KeyList.LogMessages.LogOutMessage);
                Session[KeyList.SessionKeys.UserEmail] = Session[KeyList.SessionKeys.UserType] = null;
                Session[KeyList.SessionKeys.UserID] = null;
                Session.Abandon();
                return Redirect(Url.Action("Index", "Home"));
            }
            return Redirect(Url.Action("Index", "Register_Login"));

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
            if (Session[KeyList.SessionKeys.UserID] == null)
            {

                return View();
            }
            return Redirect(Url.Action("Index", "Register_Login"));
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

        public ActionResult ChangePassword()
        {
            if (Session[KeyList.SessionKeys.UserID] != null)
            {
                return Redirect(Url.Action("MyProfile", "Register_Login"));
            }
            return Redirect(Url.Action("Index", "Register_Login"));
        }

        [HttpPost]
        public ActionResult ChangePassword(AccountModels.ChangePassword ChangePassword)
        {
            string data = "";
            if (ModelState.IsValid)
            {
                UserProfile User = UserProfileService.GetByID(Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString()));

                if (UserProfileService.Decryptdata(User.Password) == ChangePassword.CHOldPassword)
                {
                    if (UserProfileService.Decryptdata(User.Password) != ChangePassword.CHNewPassword)
                    {
                        User.Password = UserProfileService.PasswordEncrypt(ChangePassword.CHNewPassword);
                        UserProfileService.UpdateUser(User);
                        Logs GenerateLog = new Logs();
                        GenerateLog.CreateLog(User.UserId, KeyList.LogMessages.ChangePassword);
                        data = ViewBag.ChangePasswordError = "Password Changed Sucessfully";
                        //data = ;    
                        return Redirect(Url.Action("MyProfile", "Register_Login"));
                    }
                    ViewBag.ChangePasswordError = "Old Password is same with New password";
                    return View("MyProfile");
                }
                ViewBag.ChangePasswordError = "Wrong Password";
                return View("MyProfile");
            }

            return View("MyProfile");
        }


        public ActionResult MyProfile()
        {
            if (Session[KeyList.SessionKeys.UserID] != null)
            {

                var user = UserProfileService.GetByID(Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString()));
                EditProfile EditUser = new EditProfile()
                {
                    UsrID = user.UserId,
                    Usrtype = user.UserType1.UserTypeName,
                    UserEmail = user.Email,
                    UserName = user.Name,
                    UserNumber = (user.Contact != null ? user.Contact.Substring(3) : null),
                    Street = user.Street,
                    Surbrb = user.Suburb,
                    City = user.City,
                    Region = user.Region,
                    Country = "New Zealand",
                    PinCode = user.PinCode.ToString()
                };
                return View(EditUser);

            }
            return Redirect(Url.Action("Index", "Register_Login"));
        }

        [HttpPost]
        public ActionResult MyProfile(AccountModels.EditProfile MyProfile)
        {
            if (ModelState.IsValid)
            {
                if (Checknumber(MyProfile.UserNumber) == false)
                {
                    ViewBag.ProfileError = "Please Add Number without +64 or 0";
                    return View("MyProfile", MyProfile);
                }

                UserProfile user = UserProfileService.GetByID(MyProfile.UsrID);

                user.UserId = MyProfile.UsrID;
                user.Name = MyProfile.UserName;
                user.Contact = "+64" + MyProfile.UserNumber;
                user.Street = MyProfile.Street;
                user.Suburb = MyProfile.Surbrb;
                user.City = MyProfile.City;
                user.Region = MyProfile.Region;
                user.Country = "New Zealand";
                user.PinCode = Convert.ToInt32(MyProfile.PinCode);

                if (UserProfileService.UpdateUser(user) == true)
                {
                    Logs GenerateLog = new Logs();

                    GenerateLog.CreateLog(user.UserId, KeyList.LogMessages.ChangeProfile);
                    return Redirect(Url.Action("Index", "Register_Login"));
                }

            }
            return View("MyProfile", MyProfile);
        }

        public bool Checknumber(string number)
        {
            if (number.ToString().StartsWith("+64") || number.ToString().StartsWith("64") || number.ToString().StartsWith("0"))
            {
                return false;
            }
            return true;
        }
    }
}