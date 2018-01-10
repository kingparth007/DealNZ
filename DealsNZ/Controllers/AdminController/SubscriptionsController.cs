using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;


namespace DealsNZ.Controllers.AdminController
{
    [CustomAuthorize(KeyList.Users.Admin)]
    public class SubscriptionsController : Controller
    {
        private DealsDB db = new DealsDB();
        // private UnitOfWorks unitOfworks = new UnitOfWorks(new DealsDB());
        // GET: Subscriptions
        private ISubscription SubscriprinService = new SubscriptionServices(new DealsDB());
        public ActionResult Index()
        {
            return View(SubscriprinService.GetAll());
        }

        // GET: Subscriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // GET: Subscriptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubscriptionId,SubscriptionTitle,SubscriptionDiscription,SubscriptionPrice,AddedOn")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                var checkName = SubscriprinService.GetSubscriptionNameForCheck(subscription.SubscriptionTitle.ToString());
                ViewBag.ErrorMsg = "Data Already Exist";
                if (checkName == null)
                {
                    subscription.AddedOn = System.DateTime.Now.Date;
                    SubscriprinService.Insert(subscription);
                    SubscriprinService.SaveChange();
                    ViewBag.ErrorMsg = "Data Added Sucessfully";
                    return RedirectToAction("Index");
                }

            }

            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = SubscriprinService.GetByID(Convert.ToInt32(id));
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubscriptionId,SubscriptionTitle,SubscriptionDiscription,SubscriptionPrice,AddedOn")] Subscription subscription)
        {

            ViewBag.ErrorMsg = "Data Already Exist";
            if (ModelState.IsValid)
            {
                var checkName = SubscriprinService.GetSubscriptionNameForCheck(subscription.SubscriptionTitle);
                if (checkName.SubscriptionId == subscription.SubscriptionId || checkName == null)
                {


                    SubscriprinService.UpdateSubscription(subscription);
                    SubscriprinService.SaveChange();
                    return RedirectToAction("Index");
                }
            }
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Subscription subscription = unitOfworks.Subscription.GetClassByID(Convert.ToInt32(id));
        //    if (subscription == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(subscription);
        //}

        // POST: Subscriptions/Delete/5
        // [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscription subscription = SubscriprinService.GetByID(Convert.ToInt32(id));
            SubscriprinService.Delete(subscription);
            SubscriprinService.SaveChange();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
