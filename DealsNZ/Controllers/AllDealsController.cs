using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealsNZ.Controllers
{
    public class AllDealsController : Controller
    {
        // GET: AllDeals
        public ActionResult Index()
        {
            return View();
        }
    }
}