using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using static DealsNZ.Models.DealsModels;
using DealsNZ.DealPayment;
using DealsNZ.Helpers;
using PayPal.Api;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Models.Repository.ClassServices;

namespace DealsNZ.Controllers.UserController
{
    public class WalletController : Controller
    {
        IUserWallet walleservice;
        public Dictionary<string, string> config;
        OAuthTokenCredential auth;
        // GET: Wallet
        public ActionResult Index()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return Redirect(Url.Action("Index", "Register_Login"));
            }
            return View();
        }
        [HttpGet]
        public ActionResult AddMoney()
        {
            if (Session[KeyList.SessionKeys.UserID] == null)
            {
                return Redirect(Url.Action("Index", "Register_Login"));
            }
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
        public ActionResult PaymentSucess()
        {
            //if ((string)Session["email"] == null)
            //{
            //    Response.Redirect("/Home?Login=true");
            //    Session.Clear();
            //}

            try
            {
                var paymentId = Request.Params["paymentId"].ToString();
                var token = Request.Params["token"].ToString();
                var payerid = Request.Params["PayerID"].ToString();

                // Using the information from the redirect, setup the payment to execute.
                var paymentExecution = new PaymentExecution() { payer_id = payerid };
                var payment = new PayPal.Api.Payment() { id = paymentId };

                config = PayPal.Api.ConfigManager.Instance.GetProperties();
                auth = new OAuthTokenCredential(config);
                var apiContext = new APIContext(auth.GetAccessToken());

                //                BetaDB db = new BetaDB();

                int UserID = Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString());
                //              var login = db.Logins.Where(x => x.Email == email).FirstOrDefault();


                var executedPayment = payment.Execute(apiContext, paymentExecution);
                if (executedPayment.failed_transactions == null)
                {
                    List<Transaction> tl = executedPayment.transactions;
                    foreach (Transaction transaction in tl)
                    {
                        String invoice = transaction.invoice_number;
                        //    Payment pay = db.Payments.Where(x => x.InvoiceNo == invoice).SingleOrDefault();
                        //   pay.Status = 1;
                        ItemList itemlist = transaction.item_list;
                        foreach (Item i in transaction.item_list.items)
                        {
                            try
                            {
                                int id = Convert.ToInt16(i.sku);
                                int q = Convert.ToInt32(i.quantity);
                                Decimal p = Convert.ToDecimal(i.price);
                               
                                walleservice = new UserWalletServices(new DealsDB());
                                Wallet AddTrans = walleservice.GetCreditByUserID(Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString()));

                                AddTrans.UserId = Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString());
                                AddTrans.WalletCredit = Convert.ToDecimal( p+ Convert.ToDecimal(AddTrans.WalletCredit));
                                AddTrans.WalletCreditDate = System.DateTime.Now;


                                if (walleservice.WalletUpdate(AddTrans) == true)
                                {
                                    Session[KeyList.SessionKeys.WalletCredit] = walleservice.ShowWalletAmount(Convert.ToInt32(Session[DealsNZ.Helpers.KeyList.SessionKeys.UserID].ToString()));
                                    return RedirectToAction("Index", "Home");
                                }
                                walleservice.Dispose();
                               


                            }
                            catch (Exception er)
                            {

                            }

                        }
                        //// Send Email to user

                        //Services.MethodHandler.Sendemail(Session["email"].ToString(), "OrderSuccess", transaction.invoice_number);
                        ////
                        //paymentlbl.Text = "We recieved you payment nd your order is being processed";
                    }
                }
                // db.SaveChanges();
            }
            catch (Exception error)
            {

            }

            return View();
        }
        public void ShowWalletAmount()
        {
            walleservice = new UserWalletServices(new DealsDB());
            Session[KeyList.SessionKeys.WalletCredit] = ViewBag.WalletAmount = walleservice.ShowWalletAmount(Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString()));
                   }
    }
}