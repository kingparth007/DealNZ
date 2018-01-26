using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.DealPayment;
using DealsNZ.Helpers;
using DealsNZ.Models;

namespace DealsNZ.Controllers.UserController
{
    public class WalletController : Controller
    {
        // GET: Wallet
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddMoney()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMoney(WalletModel.AddMoney AddMoney)
        {
            int Amount = AddMoney.EnterMoney;
            PaypalItem p = new PaypalItem();
            p.currency = "NZD";
            p.sku = Session[KeyList.SessionKeys.UserID].ToString();
            p.price = Convert.ToDouble(Amount);

            List<PaypalItem> ListPay = new List<PaypalItem>();
            ListPay.Add(p);
            PaymentHandler ph = new PaymentHandler(ListPay, Convert.ToInt32(Session[KeyList.SessionKeys.UserID]));
            return View();
        }
    }
}