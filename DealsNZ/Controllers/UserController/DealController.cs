using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;
using static DealsNZ.Models.DealsModels;

namespace DealsNZ.Controllers.UserController
{
    public class DealController : Controller
    {
        IDeal dealServices;
        // GET: Deal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewDeal()
        {
            if (RouteData.Values["id"] != null)
            {
                int ID = Convert.ToInt32(RouteData.Values["id"].ToString());
                dealServices = new DealServices(new DealsDB());
                Deal getDeal = dealServices.GetByID(ID);
                ViewSingleDeal SingleDeal = new ViewSingleDeal();
                SingleDeal.DealId = getDeal.DealId;
                SingleDeal.DealImages = getDeal.DealImages.FirstOrDefault().DealImage1;
                SingleDeal.Price = Convert.ToInt32(getDeal.Price);
                SingleDeal.Description = getDeal.Description;
                SingleDeal.Discount = Convert.ToInt32(getDeal.Discount);
                SingleDeal.ValidTill = DateTime.Parse(getDeal.ValidTill.ToString());
                SingleDeal.Title = getDeal.Title;
                SingleDeal.StrikePrice = Convert.ToInt32(getDeal.StrikePrice);
                SingleDeal.CouponPrice = Convert.ToInt32(getDeal.StrikePrice);
                return View(SingleDeal);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}