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

        public ActionResult CouponGenerator(ViewSingleDeal CreateCoupon)
        {

            string CouponCode = CreateCoupon.DealId.ToString() + GenerateCode();
            
            return View();
        }


        private string GenerateCode()
        {
            int lenthofOtp = 6;
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string otpString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < lenthofOtp; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                otpString += temp;
            }
            return otpString;
        }
    }


}