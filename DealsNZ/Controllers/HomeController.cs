using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepoPattern.Models.RepositoryFiles;
using DealsNZ.Models;

namespace DealsNZ.Controllers
{
    public class HomeController : Controller
    {
       // UnitOfWorks Un = new UnitOfWorks(new DealsDB());
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}