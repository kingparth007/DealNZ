using System;
using System.Collections.Generic;
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
    public class UserTypeController : Controller
    {
        //private UnitOfWorks unitOfworks = new UnitOfWorks(new DealsDB());
        // GET: UserType
        private IUserType UsertypeService = new UserTypeService(new DealsDB());
        public ActionResult Index()
        {

            return View(UsertypeService.GetAll().ToList());
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
                    var getusertypename = UsertypeService.GetUserByName(usertype.UserTypeName);
                    if (getusertypename == null)
                    {
                        UsertypeService.Insert(usertype);
                        UsertypeService.SaveChange();

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
            UserType usertype = UsertypeService.GetByID(id);
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
                    UserType getusertypename = UsertypeService.GetUserByName(usertype.UserTypeName);
                    if (getusertypename == null || usertype.UserTypeId == getusertypename.UserTypeId)
                    {

                        UsertypeService.UpdateUserType(usertype);
                        UsertypeService.SaveChange();
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
                    UserType usertype = UsertypeService.GetByID(id);
                    if (usertype == null)
                    {
                        return HttpNotFound();
                    }
                    UsertypeService.Delete(usertype);
                    UsertypeService.SaveChange();
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