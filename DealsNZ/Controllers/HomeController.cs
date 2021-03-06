﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;



namespace DealsNZ.Controllers
{
    public class HomeController : Controller
    {
        // UnitOfWorks Un = new UnitOfWorks(new DealsDB());
        IDeal DealService = new DealServices(new DealsDB());

        public ActionResult Index()
        {

            var DealList = DealService.AllDeal().Where(x=>x.IsDeleted==false).ToList();
            return View(DealList);
        }

        //Search Deal
        public ActionResult SearchText()
        {
            if (RouteData.Values["id"] != null)
            {
                string id = RouteData.Values["id"].ToString();
                var DealList = DealService.AllDeal().Where(x => x.Title.ToLower().Contains(id.ToLower()) && x.IsDeleted == false).ToList();
                return View("Index",DealList);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Slide()
        {
            ViewBag.Message = "Your application description page.";

            var DealList = DealService.AllDeal().OrderBy(x => Guid.NewGuid()).Take(5);
            return View(DealList);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}