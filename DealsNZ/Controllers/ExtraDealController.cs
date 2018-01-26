using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;

namespace DealsNZ.Controllers
{
    public class ExtraDealController : Controller
    {
        private DealsDB db = new DealsDB();

        // GET: ExtraDeal
        public ActionResult Index()
        {
            var deals = db.Deals.Include(d => d.Store);
            return View(deals.ToList());
        }

        // GET: ExtraDeal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // GET: ExtraDeal/Create
        public ActionResult Create()
        {
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName");
            return View();
        }

        // POST: ExtraDeal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DealId,StoreId,ValidTill,Discount,StrikePrice,Price,Title,IsDealFree,Description,AddedOn,IsDeleted")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", deal.StoreId);
            return View(deal);
        }

        // GET: ExtraDeal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", deal.StoreId);
            return View(deal);
        }

        // POST: ExtraDeal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DealId,StoreId,ValidTill,Discount,StrikePrice,Price,Title,IsDealFree,Description,AddedOn,IsDeleted")] Deal deal)
        {
            if (ModelState.IsValid)
            {
               // db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Stores, "StoreId", "StoreName", deal.StoreId);
            return View(deal);
        }

        // GET: ExtraDeal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: ExtraDeal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
            db.SaveChanges();
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
