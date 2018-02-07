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
     [CustomAuthorize(KeyList.Users.Admin)]
    public class AdminController : Controller
    {
        // GET: Admin
        ICoupon couponService;

        IStore storeService = new StoreServices(new DealsDB());
        IAddress addressService = new AddressService(new DealsDB());
        ICompany companyService = new CompanyService(new DealsDB());
        IDeal dealServices = new DealServices(new DealsDB());
        IDealImage dealImageServices = new DealImageService(new DealsDB());
        IUserProfile userProfileService = new UserProfileServices(new DealsDB());

        #region StoreSection


        public ActionResult Store(int? page, string searchBy, string search)
        {

            return View(storeService.Get(x => x.StoreName.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 2));

            //var listofStore = storeService.GetAll().ToPagedList(page ?? 1, 2);
            //return View(listofStore);
        }

        // GET: Store/Create
        public ActionResult CreateStore()
        {
            StoreViewModel dropdown = DropdownforCompany();

            return View(dropdown);
        }

        private StoreViewModel DropdownforCompany()
        {
            StoreViewModel dropdown = new StoreViewModel();
            dropdown.CompanyList = companyService.GetAll().
               Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
               ToList();
            return dropdown;
        }

        // POST: Store/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStore(StoreViewModel store, IEnumerable<HttpPostedFileBase> files)
        {
            // to create dropdownlist

            StoreViewModel dropdown = DropdownforCompany();
            try
            {
                if (Session[KeyList.SessionKeys.UserID] != null)
                {
                    var userId = Session[KeyList.SessionKeys.UserID].ToString();
                    if (ModelState.IsValid)
                    {
                        if (Checknumber(store.Contact) == false)
                        {
                            ViewBag.message = "Please Add Number without +64 or 0";
                            return View(dropdown);
                        }
                        var addressss = addressService.Get(a => a.City == store.City && a.Street == store.Street && a.Country == store.Country).SingleOrDefault();
                        if (addressss == null)
                        {
                            bool Image = files.IsValidImageList(false);
                            if (Image == true)
                            {
                                foreach (var item in files)
                                {
                                    var path = item.SaveImageFile();

                                    Store _store = new Store
                                    {
                                        StoreName = store.StoreName,
                                        UserId = Convert.ToInt32(userId),
                                        Contact = "+64" + store.Contact,
                                        IdentificationImage = path,
                                        CompanyId = store.CompanyId,
                                        IsValid = false,
                                        IsDeleted = false,

                                    };

                                    int id = storeService.CreateStore(_store);
                                    Address address = new Address
                                    {
                                        Street = store.Street,
                                        City = store.City,
                                        Country = store.Country,
                                        StoreId = id,
                                    };
                                    addressService.CreateAddress(address);
                                }
                            }
                            else
                            {
                                ViewBag.message = "Image is required and should only have .pdf,.jpeg,.jpg,.gif";
                                return View(dropdown);
                            }
                        }
                        Logs GenerateLog = new Logs();
                        GenerateLog.CreateLog(Convert.ToInt32(userId), KeyList.LogMessages.CreateStore);
                        return RedirectToAction("Store");


                    }
                }

                return View(dropdown);
            }
            catch (Exception e)
            {
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
            dropdown.CompanyList = companyService.GetAll().
Select(p => new CompanyViewModel { CompanyId = p.CompanyId, CompanyName = p.CompanyName }).
ToList();

            return View(dropdown);
        }

        // POST: Store/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStore(StoreViewModel store)
        {
            StoreViewModel dropdown = DropdownforCompany();
            try
            {
                if (ModelState.IsValid)
                {

                    if (Checknumber(store.Contact) == false)
                    {
                        ViewBag.message = "Please Add Number without +64 or 0";
                        return View(dropdown);
                    }
                    var address = addressService.GetAddressBystoreId(store.StoreId).FirstOrDefault();

                    Store _store = storeService.GetByID(store.StoreId);
                    _store.StoreId = store.StoreId;
                    _store.StoreName = store.StoreName;
                    _store.Contact = "+64" + store.Contact;
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
                    Logs GenerateLog = new Logs();
                    GenerateLog.CreateLog(Convert.ToInt32(_store.UserId), KeyList.LogMessages.EditStore);
                }

                return RedirectToAction("Store");
            }
            catch (Exception e)
            {
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
                var _store = storeService.GetByID(id);
                if (_store.IsValid == false)
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
                Logs GenerateLog = new Logs();
                GenerateLog.CreateLog(Convert.ToInt32(_store.UserId), KeyList.LogMessages.ValidateStore);
                return RedirectToAction("Store");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteStore(int id)
        {
            var _store = storeService.GetByID(id);
            return View(_store);
        }

        [HttpPost]
        public ActionResult DeleteStore(int id, FormCollection collection)
        {
            var _store = storeService.GetByID(id);
            if (_store.IsDeleted == false)
            {
                Store store = storeService.GetByID(id);
                store.IsDeleted = true;
                storeService.UpdateStore(store);
            }
            else
            {

                Store store = storeService.GetByID(id);
                store.IsDeleted = false;
                storeService.UpdateStore(store);
            }
            Logs GenerateLog = new Logs();
            GenerateLog.CreateLog(Convert.ToInt32(_store.UserId), KeyList.LogMessages.DeleteStore);
            return RedirectToAction("Store");

        }

        #endregion

        #region Company
        public ActionResult CreateCompany()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompany(CompanyViewModel company)
        {
            Company comp = new Company();
            comp.CompanyName = company.CompanyName.ToUpper();
            companyService.CreateCompany(comp);
            return RedirectToAction("Store");
        }




        #endregion
        #region Deals Section
        // GET: Deal
        public ActionResult Deal(int? page)
        {
            var listofDeals = dealServices.GetAll().ToPagedList(page ?? 1, 2); ;
            return View(listofDeals);
        }

        // GET: Deal/Create
        public ActionResult CreateDeal()
        {
            var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
            DealViewModel dropdown = DropDownForstore(userId);
            return View(dropdown);
        }

        private DealViewModel DropDownForstore(int userId)
        {
            DealViewModel dropdown = new DealViewModel();
            dropdown.StoreList = storeService.Get(x => x.UserProfile.UserId == userId).
              Select(p => new StoreViewModel { StoreId = p.StoreId, StoreName = p.StoreName }).
              ToList();
            return dropdown;
        }

        // POST: Deal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeal(DealViewModel _deal, IEnumerable<HttpPostedFileBase> files)
        {
            var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());

            DealViewModel dropdown = DropDownForstore(userId);
            try
            {
                if (ModelState.IsValid)
                {
                    var stores = storeService.Get(x => x.UserId == userId);
                    var image = files.IsValidImageList(false);
                    if (image == false)
                    {
                        ViewBag.message = "Image is required and should only have .pdf,.jpeg,.jpg,.gif";
                        return View(dropdown);
                    }
                    else
                    {
                        foreach (var store in stores)
                        {
                            if (store.StoreId == _deal.StoreId)
                            {
                                var strikePrice = _deal.Price - (decimal)((double)_deal.Discount / 100 * _deal.Price);
                                if (store.IsValid == true)
                                {
                                    Deal deal = new Deal
                                    {
                                        StoreId = _deal.StoreId,
                                        Description = _deal.Description,
                                        Title = _deal.Title,
                                        ValidTill = _deal.ValidTill.Date,
                                        Price = _deal.Price,
                                        Discount = _deal.Discount,
                                        StrikePrice = strikePrice,
                                        AddedOn = DateTime.Now,
                                        IsDeleted = false,
                                        IsDealFree = _deal.IsDealfree

                                    };
                                    int id = dealServices.CreateDeal(deal);

                                    foreach (var item in files)
                                    {
                                        var path = item.SaveImageFile();
                                        DealImage _dealImage = new DealImage()
                                        {
                                            DealId = id,
                                            DealImage1 = path
                                        };

                                        dealImageServices.CreateDealImage(_dealImage);
                                    }

                                }
                                else
                                {
                                    ViewBag.message = "your store registration certificate is not valid";
                                    return View(dropdown);
                                }
                            }



                        }
                    }
                    Logs GenerateLog = new Logs();
                    GenerateLog.CreateLog(userId, KeyList.LogMessages.EditStore);
                    return RedirectToAction("Deal");
                }
                return View(dropdown);
            }
            catch (Exception e)
            {
                return View(dropdown);
            }
        }

        public ActionResult EditDeal(int id)
        {

            Deal deal = dealServices.GetByID(id);
            var Images = deal.DealImages;
            DealViewModel dropdown = new DealViewModel
            {
                DealId = deal.DealId,
                Title = deal.Title,
                Price = Convert.ToInt32(deal.Price),
                Discount = Convert.ToInt32(deal.Discount),
                ValidTill = DateTime.Parse(deal.ValidTill.ToString()),
                Description = deal.Description,
            };
            return View(dropdown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDeal(DealViewModel deal)
        {

            try
            {

                var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
                if (ModelState.IsValid)
                {
                    var strikePrice = deal.Price - (decimal)((double)deal.Discount / 100 * deal.Price);
                    Deal _deal = dealServices.GetByID(deal.DealId);
                    _deal.Title = deal.Title;
                    _deal.Price = deal.Price;
                    _deal.Discount = deal.Discount;
                    _deal.ValidTill = deal.ValidTill;
                    _deal.Description = deal.Description;
                    _deal.StrikePrice = strikePrice;
                    dealServices.UpdateDeal(_deal);
                    Logs GenerateLog = new Logs();
                    GenerateLog.CreateLog(userId, KeyList.LogMessages.EditStore);
                }
                return RedirectToAction("Deal");
            }
            catch (Exception)
            {

                return View(deal);
            }


        }

        public ActionResult DeleteDeal(int id)
        {
            var _deal = dealServices.GetByID(id);
            return View(_deal);
        }

        [HttpPost]
        public ActionResult DeleteDeal(int id, FormCollection collection)
        {
            var userId = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
            var _deal = dealServices.GetByID(id);
            if (_deal.IsDeleted == false)
            {
                Deal deal = dealServices.GetByID(id);
                deal.IsDeleted = true;
                dealServices.UpdateDeal(deal);
            }
            else
            {
                Deal deal = dealServices.GetByID(id);
                deal.IsDeleted = false;
                dealServices.UpdateDeal(deal);
            }
            Logs GenerateLog = new Logs();
            GenerateLog.CreateLog(Convert.ToInt32(userId), KeyList.LogMessages.DeleteStore);
            return RedirectToAction("Deal");

        }

        public ActionResult Image(int Id)
        {
            var images = dealImageServices.GetAll().Where(x => x.DealId == Id);
            return View(images);
        }

        public ActionResult Coupon(string searchBy, string search)
        {
            couponService = new CouponService(new DealsDB());

            return View(couponService.Get(x => x.CouponUniqueText.StartsWith(search) || search == null).ToList());
        }


        public ActionResult RedeemCoupon(int Id)
        {
            couponService = new CouponService(new DealsDB());

            Coupon cupon = couponService.GetByID(Id);
            if (cupon.CouponValidTill <= DateTime.Now)
            {

                TempData["Message"] = "The deal <b> " + cupon.CouponUniqueText + "</b> you trying to redeeem is already expired";
            }
            else
            {
                if (cupon.ReedemNo < cupon.CouponQty)
                {
                    cupon.ReedemNo = cupon.ReedemNo + 1;
                    couponService.UpdateCoupon(cupon);
                    TempData["Message"] = "The coupon  " + cupon.CouponUniqueText + " is redeemed for " + cupon.ReedemNo + " times";
                }
                else
                {
                    TempData["Message"] = "The coupon " + cupon.CouponUniqueText + " not valid anymore .It was used for " + cupon.ReedemNo + " time already";
                }
            }
            return RedirectToAction("Coupon");
        }
        #endregion
        public bool Checknumber(string number)
        {
            if (number.ToString().StartsWith("+64") || number.ToString().StartsWith("64") || number.ToString().StartsWith("0"))
            {
                return false;
            }
            return true;
        }
        
  
        public ActionResult Dashboard()
        {
            int UserID = Convert.ToInt32(Session[KeyList.SessionKeys.UserID].ToString());
            int StoreCount = storeService.GetAll().Where(x=>x.UserId==UserID).Count();
            int DealCount = dealServices.GetAll().Where(x => x.Store.UserId == UserID).Count();
            int CouponCount = couponService.GetAll().Where(x => x.Deal.Store.UserId == UserID).Count();           
            return View();
        }

    }
}