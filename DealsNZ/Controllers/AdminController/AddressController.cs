using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealsNZ.Controllers.AdminController
{
    public class AddressController : Controller
    {
        IAddress addressService = new AddressService(new DealsDB());
        // GET: Address
        public ActionResult GetAllAddress()
        {
            var list = addressService.GetAll();
            return View(list);
        }
    }
}