using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;
using static DealsNZ.Models.DealsModels;

namespace DealsNZ.Controllers.UserController
{
    public class DealController : Controller
    {
        IDeal dealServices;
        ICoupon couponservice;
        IUserWallet walleservice;
        IUserWishList wishListService;
        // GET: Deal

        public ActionResult Index()
        {
            if (RouteData.Values["id"] != null)
            {

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewDeal()
        {
            if (RouteData.Values["id"] != null)
            {
                int ID = Convert.ToInt32(RouteData.Values["id"].ToString());
                dealServices = new DealServices(new DealsDB());


                ViewSingleDeal SingleDeal = dealServices.GetSingleDeal(ID);
                ViewBag.Message = " ";
                dealServices.Dispose();
                return View("Index", SingleDeal);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CouponGenerator(ViewSingleDeal CreateCoupon)
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return RedirectToAction("Index", "Register_Login");
            }

            //    walleservice = new UserWalletServices(new DealsDB());
            walleservice = new UserWalletServices(new DealsDB());
            Wallet AddTrans = walleservice.GetCreditByUserID(Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString()));

            if (CreateCoupon.CouponPrice > AddTrans.WalletCredit)
            {
                walleservice.Dispose();
            }
            else
            {
                AddTrans.UserId = Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString());
                AddTrans.WalletCredit = Convert.ToDecimal(Convert.ToDecimal(AddTrans.WalletCredit) - Convert.ToDecimal(CreateCoupon.CouponPrice));
                AddTrans.WalletCreditDate = System.DateTime.Now;

                Wallet DealUserWallet = walleservice.GetCreditByDealUserID(CreateCoupon.DealId);
                DealUserWallet.WalletCredit = Convert.ToDecimal(Convert.ToDecimal(DealUserWallet.WalletCredit) + Convert.ToDecimal(CreateCoupon.CouponPrice));

                if (walleservice.WalletUpdate(AddTrans) == true && walleservice.WalletUpdate(DealUserWallet) == true)
                {
                    Coupon InsertCoupon = new Coupon()
                    {
                        CouponUniqueText = CreateCoupon.DealId.ToString() + GenerateCode(),
                        CouponValidTill = CreateCoupon.ValidTill,
                        CouponQty = CreateCoupon.CouponQty,
                        CouponPrice = CreateCoupon.CouponPrice,
                        AddedOn = System.DateTime.Now.Date,
                        DealId = CreateCoupon.DealId,
                        UserId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID]),
                        ReedemNo = 0
                    };
                    couponservice = new CouponService(new DealsDB());
                    couponservice.Insert(InsertCoupon);
                    couponservice.Dispose();
                    Session[KeyList.SessionKeys.WalletCredit] = walleservice.ShowWalletAmount(Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString()));
                    walleservice.Dispose();
                    string CouponBody = createEmailBody(CreateCoupon.StoreName, CreateCoupon.Address, CreateCoupon.Title, CreateCoupon.Price.ToString(), CreateCoupon.StrikePrice.ToString(), CreateCoupon.Discount.ToString(), InsertCoupon.CouponQty.ToString(), InsertCoupon.CouponValidTill.ToString(), InsertCoupon.CouponUniqueText);
                    string Title = "Coupon For " + CreateCoupon.Title;
                    IUserProfile UserProfileService = new UserProfileServices(new DealsDB());
                    if (UserProfileService.UserMail(CouponBody, Title, Session[KeyList.SessionKeys.UserEmail].ToString()) == true)
                    {
                        dealServices = new DealServices(new DealsDB());
                        ViewSingleDeal SingleDeal = dealServices.GetSingleDeal(CreateCoupon.DealId);
                        dealServices.Dispose();
                        ViewBag.Message = "Check Your Mail To Get Coupon";
                        return View("Index", SingleDeal);

                    }
                }
                else
                {
                    walleservice.Dispose();
                }
            }

            return View();
        }

        public ActionResult RelatedDeal()
        {

            return View();

        }

        public IEnumerable<DealsModels.DealViewModel> RelatedDeals(int DealID)
        {

            dealServices = new DealServices(new DealsDB());

            IEnumerable<DealsModels.DealViewModel> DealList = dealServices.AllDeal().Where(x => x.DealId != DealID).OrderBy(x => Guid.NewGuid()).Take(3);
            return DealList;

        }

        public ActionResult ShowCoupons()
        {
            if (Session[KeyList.SessionKeys.UserID] != null)
            {
                couponservice = new CouponService(new DealsDB());
                var couponList = couponservice.ViewCoupons(Convert.ToInt32(Session[KeyList.SessionKeys.UserID]));
                return View(couponList);
            }
            return RedirectToAction("Index", "Register_Login");
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

        private string createEmailBody(string StoreName, string Address, string DealTitle, string Price, string StrikePrice, string Discount, string Qty, string Expire, string Code)

        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemp/CouponCodeTemplate.html")))

            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{DealTitle}", DealTitle); //replacing the required things  
            body = body.Replace("{Price}", Price);
            body = body.Replace("{StrikePrice}", StrikePrice);
            body = body.Replace("{Discount}", Discount);
            body = body.Replace("{Qty}", Qty);
            body = body.Replace("{Expire}", Expire);
            body = body.Replace("{Code}", Code);// 
            body = body.Replace("{Address}", Address);
            body = body.Replace("{Store}", StoreName);// 

            return body;

        }

        public ActionResult InsertInWishList()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return RedirectToAction("Index", "Register_Login");
            }
            if (RouteData.Values["id"] != null)
            {

                int DealID = Convert.ToInt32(RouteData.Values["id"].ToString());

                WishList InsWishList = new WishList();
                InsWishList.DealId = DealID;
                InsWishList.UserId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
                InsWishList.AddedOn = System.DateTime.Now;
                wishListService = new UserWishListService(new DealsDB());
                if (wishListService.wishlistCheck(DealID, InsWishList.UserId) == true)
                {
                    wishListService.Insert(InsWishList);
                    ViewBag.Message = "Added to WishList";

                }
                else
                {
                    ViewBag.Message = "Already Added";
                }

                wishListService.Dispose();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewWishList()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return RedirectToAction("Index", "Register_Login");
            }
            wishListService = new UserWishListService(new DealsDB());
            var List = wishListService.ViewWishListByUser(Convert.ToInt32(Session[KeyList.SessionKeys.UserID]));
            wishListService.Dispose();
            return View(List);
        }

        public ActionResult RemoveFromWishList()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return RedirectToAction("Index", "Register_Login");
            }
            if (RouteData.Values["id"] != null)
            {
                int DealID = Convert.ToInt32(RouteData.Values["id"].ToString());
                int UserID = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
                wishListService = new UserWishListService(new DealsDB());
                WishList DelWishList = wishListService.GetAll().Where(x => x.DealId == DealID && x.UserId == UserID).FirstOrDefault();
                wishListService.Delete(DelWishList);
                wishListService.Dispose();
                return View("ViewWishList");
            }
            else
            {
                return View("ViewWishList");
            }

        }
    }

}