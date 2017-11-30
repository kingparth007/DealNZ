using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DealsNZ.Models.StoreModel;
using PagedList;
using PagedList.Mvc;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Helpers;

namespace DealsNZ.Controllers
{
    [CustomAuthorize(KeyList.UserType.Admin)]
    public class StoreController : Controller
    {
        //TODO UserId need to Insert
        IStore storeService = new StoreServices(new DealsDB());
        IAddress addressService = new AddressService(new DealsDB());
        ICompany companyService = new CompanyService(new DealsDB());
        IUserProfile userProfileService = new UserProfileServices(new DealsDB());
        // GET: Store
        public ActionResult Index(int? page)
        {
            return View(storeService.GetAllStores().ToPagedList(page ?? 1, 5));
        }

        // GET: Store/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            StoreViewModel dropdown = new StoreViewModel();
            dropdown.CompanyList = companyService.GetAllCompany().
               Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
               ToList();
            //ViewBag.Companies = new SelectList(companyService.GetAllCompany(), "CompanyId", "CompanyName");
            return View(dropdown);
        }

        // POST: Store/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreViewModel store)
        {
            // to create dropdownlist
            StoreViewModel dropdown = new StoreViewModel();
            dropdown.CompanyList = companyService.GetAllCompany().
               Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
               ToList();

            try
            {
                if (Session[KeyList.SessionKeys.UserID] != null)
                {
                    var userId = Session[KeyList.SessionKeys.UserID].ToString();
                    string fileName = Path.GetFileNameWithoutExtension(store.Image.FileName);
                    string extension = Path.GetExtension(store.Image.FileName);
                    fileName = fileName + extension;
                    store.IdentificationImage = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    store.Image.SaveAs(fileName);

                    if (ModelState.IsValid)
                    {
                        Store _store = new Store
                        {
                            StoreName = store.StoreName,
                            UserId = Convert.ToInt32(userId),
                            Contact = store.Contact,
                            IdentificationImage = store.IdentificationImage,
                            CompanyId = store.CompanyId
                        };

                        int id = storeService.CreateStore(_store);
                        //storeService.Insert(_store);
                        //to add address

                        Address address = new Address
                        {
                            Street = store.Street,
                            City = store.City,
                            Country = store.Country,
                            StoreId = id,
                        };
                        addressService.CreateAddress(address);

                        return RedirectToAction("Index");
                    }
                }
                return View(dropdown);
            }
            catch (Exception e)
            {
                var exception = e;

                return View(dropdown);
            }
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int id)
        {         

            Store store = storeService.GetStoreById(id);
            var address = addressService.GetAddressBystoreId(id).SingleOrDefault();
            StoreViewModel dropdown = new StoreViewModel
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                Contact = store.Contact,
                CompanyId = store.Company.CompanyId,               
                IdentificationImage = store.IdentificationImage,
                City = address.City,
                Country = address.Country,
                Street = address.Street
            };
            dropdown.CompanyList = companyService.GetAllCompany().
            Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
            ToList();

            return View(dropdown);
        }

        // POST: Store/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoreViewModel store)
        {
            StoreViewModel dropdown = new StoreViewModel();
            dropdown.CompanyList = companyService.GetAllCompany().
               Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
               ToList();
            try
            {
                if (ModelState.IsValid)
                {

                    var address = addressService.GetAddressBystoreId(store.StoreId).FirstOrDefault();

                    //string fileName = Path.GetFileNameWithoutExtension(store.Image.FileName);
                    //string extension = Path.GetExtension(store.Image.FileName);
                    //fileName = fileName + extension;
                    //store.IdentificationImage = "~/Images/" + fileName;
                    //fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    //store.Image.SaveAs(fileName);

                    Store _store = storeService.GetByID(store.StoreId);
                         _store.StoreId = store.StoreId;
                        _store.StoreName = store.StoreName;
                       
                        _store.Contact = store.Contact;
                       
                        _store.CompanyId = store.CompanyId;
                   
                    storeService.UpdateStore(_store);
                    if (address.StoreId == store.StoreId)
                    {
                        address.AddressId = address.AddressId;
                        address.Street = store.Street;
                        address.City = store.City;
                        address.Country = store.Country;
                        addressService.UpdateAddress(address);
                    };
                 
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var aa = e;
                return View(dropdown);
            }
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int id)
        {
            return View(storeService.GetStoreById(id));
        }

        // POST: Store/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                storeService.RemoveStorebyId(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
