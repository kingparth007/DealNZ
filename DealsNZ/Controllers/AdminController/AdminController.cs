using DealsNZ.Helpers;
using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DealsNZ.Models.DealsModels;

namespace DealsNZ.Controllers.AdminController
{
    public class AdminController : Controller
    {
        // GET: Admin

        IStore storeService = new StoreServices(new DealsDB());
        IAddress addressService = new AddressService(new DealsDB());
        ICompany companyService = new CompanyService(new DealsDB());
        IDeal dealServices = new DealServices(new DealsDB());
        IDealImage dealImageServices = new DealImageService(new DealsDB());
        IUserProfile userProfileService = new UserProfileServices(new DealsDB());
        // GET: Store

        // [CustomAuthorize(KeyList.UserType.Admin)]
        public ActionResult Store(int? page)
        {
            return View(storeService.Get().ToPagedList(page ?? 1, 5));
        }


        //[CustomAuthorize(KeyList.UserType.Vendor)]
        //public ActionResult VendorStore()
        //{
        //    var usertype = Session[KeyList.SessionKeys.UserType].ToString();
        //    var userId = Session[KeyList.SessionKeys.UserID].ToString();
        //    var user = userProfileService.GetByID(Convert.ToInt32(userId));
        //    var store = storeService.Get(x => x.UserId == user.UserId && x.UserProfile.UserType1.UserTypeName == usertype);
        //    return View(store);
        //}

        // GET: Store/Create
        public ActionResult CreateStore()
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
        public ActionResult CreateStore(StoreViewModel store)
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
                    store.IdentificationImage = "~/Images/Certificates" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Certificates"), fileName);
                    store.Image.SaveAs(fileName);

                    if (ModelState.IsValid)
                    {
                        var addressss = addressService.Get(a => a.City == store.City && a.Street == store.Street && a.Country == store.Country).SingleOrDefault();
                        if (addressss == null)
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

                            return RedirectToAction("Store");
                        }

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
        public ActionResult EditStore(int id)
        {

            Store store = storeService.GetByID(id);
            var address = addressService.GetAddressBystoreId(id).SingleOrDefault();
            StoreViewModel dropdown = new StoreViewModel
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                Contact = store.Contact,
                CompanyId = store.Company.CompanyId,
                UserId = store.UserId,
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
        public ActionResult EditStore(StoreViewModel store)
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

                return RedirectToAction("Store");
            }
            catch (Exception e)
            {
                var aa = e;
                return View(dropdown);
            }
        }

        // GET: Store/Delete/5
        public ActionResult Validate(int id)
        {
            return View(storeService.GetByID(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int id, FormCollection collection)
        {
            try
            {
                var IsValidStore = storeService.GetByID(id).IsValid;
                if (IsValidStore == false)
                {
                    Store store = storeService.GetByID(id);
                    store.IsValid = true;
                    storeService.UpdateStore(store);
                }
                else
                {
                    Store store = storeService.GetByID(id);
                    store.IsValid = false;
                    storeService.UpdateStore(store);

                }

                return RedirectToAction("Store");
            }
            catch
            {
                return View();
            }
        }

        // Deals Section



        // GET: Deal

        public ActionResult Deal(int? page)
        {
            var listofDeals = dealServices.AllDeal().ToPagedList(page ?? 1, 2);
            return View(listofDeals);
        }

        // GET: Deal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Deal/Create
        public ActionResult CreateDeal()
        {
            var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());

            DealViewModel dropdown = new DealViewModel();
            dropdown.StoreList = storeService.Get(x => x.UserProfile.UserId == userId).
              Select(p => new StoreViewModel { StoreId = p.StoreId, StoreName = p.StoreName }).
              ToList();
            return View(dropdown);
        }

        // POST: Deal/Create
        [HttpPost]
        public ActionResult CreateDeal(DealViewModel _deal)
        {
            try
            {
                var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
                var storeId = storeService.Get(x => x.UserId == userId).Select(x => new { x.StoreId, x.StoreName }).ToList();

                if (ModelState.IsValid)
                {
                    Deal deal = new Deal
                    {
                        StoreId = _deal.StoreId,
                        Description = _deal.Description,
                        Title = _deal.Title,
                        ValidTill = _deal.ValidTill,
                        Price = _deal.Price,
                        Discount = _deal.Discount,
                        AddedOn = DateTime.Parse(DateTime.UtcNow.ToShortDateString()),
                        IsDeleted = false,
                    };
                    int id = dealServices.CreateDeal(deal);
                    List<DealImage> image = new List<DealImage>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                            var url = "~/Images/DealsImages" + fileName;
                            DealImage _dealImage = new DealImage()
                            {
                                DealId = id,
                                DealImage1 = url
                            };
                            image.Add(_dealImage);

                            var path = Path.Combine(Server.MapPath("~/Images/DealsImages"), fileName);
                            file.SaveAs(path);
                            dealImageServices.CreateDealImage(_dealImage);
                        }
                    }
                    return RedirectToAction("Deal");
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}