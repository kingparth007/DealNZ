using System.Web.Mvc;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Controllers
{
    public class Register_LoginController : Controller
    {
        // GET: Register_Login
        UnitOfWorks unitofworks = new UnitOfWorks(new DealsDB());
        //UserProfileServices UserProfileService = new UserProfileServices(new DealsDB());
        //       IUserProfile profileservice = new IUserProfile()
        IUserProfile UserProfileService = new UserProfileServices(new DealsDB());
        public ActionResult Index()
        {
            ViewBag.RegisterError = "";
            ViewBag.LoginError = "";
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
                        return Redirect(Url.Action("Index", "Register_Login"));
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

                return Redirect(Url.Action("Index", "Register_Login"));
            }
            //return Redirect( Url.Action("Index", "Register_Login", Login));
            return View("Index", Login);
        }
    }
}