using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Controllers.AdminController
{
    public class UserTypeController : Controller
    {
        private UnitOfWorks unitOfworks = new UnitOfWorks(new DealsDB());
        // GET: UserType
        public ActionResult Index()
        {

            return View(unitOfworks.Usertype.GetAll().ToList());
        }

        // GET: UserType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserType/Create
        [HttpPost]
        public ActionResult Create(UserType usertype)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    var getusertypename = unitOfworks.Usertype.GetUserByName(usertype.UserTypeName);
                    if (getusertypename == null)
                    {
                        unitOfworks.Usertype.Insert(usertype);
                        unitOfworks.Complete();

                        return RedirectToAction("Index");
                    }
                    ViewBag.Error = "User Already Exist";
                    return View(usertype);
                }
                return View(usertype);
            }
            catch
            {
                return View(usertype);
            }

        }

        // GET: UserType/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType usertype = unitOfworks.Usertype.GetByID(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);

        }

        // POST: UserType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTypeId,UserTypeName")] UserType usertype)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserType getusertypename = unitOfworks.Usertype.GetUserByName(usertype.UserTypeName);
                    if (getusertypename == null || usertype.UserTypeId == getusertypename.UserTypeId)
                    {

                        unitOfworks.Usertype.UpdateUserType(usertype);
                        unitOfworks.Complete();
                        return RedirectToAction("Index");
                    }
                    ViewBag.Error = "User Already Exist";
                    return View(usertype);
                }
                return View(usertype);
            }
            catch
            {
                return View(usertype);
            }
        }


        // POST: UserType/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    UserType usertype = unitOfworks.Usertype.GetByID(id);
                    if (usertype == null)
                    {
                        return HttpNotFound();
                    }
                    unitOfworks.Usertype.Delete(usertype);
                    unitOfworks.Complete();
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}