using DealsNZ.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealsNZ.Models
{
    public class DealsModels
    {

        public class StoreViewModel
        {

            public int StoreId { get; set; }
            [Required(ErrorMessage = "Name is required")]
            public string StoreName { get; set; }
            public Nullable<int> UserId { get; set; }
            [Required]
            public string Contact { get; set; }
            [DisplayName("Registration Certificate")]
            public string IdentificationImage { get; set; }
            [DisplayName("Company")]
            [Required]
            public int CompanyId { get; set; }

            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string CompanyName { get; set; }
            public IEnumerable<CompanyViewModel> CompanyList { get; set; }

            //for address
            [DisplayName("Unit/Street")]
            public string Street { get; set; }
            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string City { get; set; }
            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string Country { get; set; }



            public IEnumerable<HttpPostedFileBase> files { get; set; }
        }
        public class CompanyViewModel
        {
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }

        }



        public class DealViewModel
        {

            public int DealId { get; set; }

            [DisplayName("Expire On")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
            public DateTime ValidTill { get; set; }
            [RegularExpression("^[0-9]+$", ErrorMessage = "Number does not contain alphabates")]
            public int Discount { get; set; }
            [RegularExpression("^[0-9]+$", ErrorMessage = "Number does not contain alphabates")]
            public int StrikePrice { get; set; }
            [RegularExpression("^[0-9]+$", ErrorMessage = "Number does not contain alphabates")]
            public int Price { get; set; }
            [StringLength(250, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string Title { get; set; }
            [StringLength(2000, ErrorMessage = "Length Between 0 to 2000 character", MinimumLength = 0)]
            public string Description { get; set; }
            public DateTime AddedOn { get; set; }
            public bool IsDeleted { get; set; }
            public int StoreId { get; set; }
            [DisplayName("Free")]
            public bool IsDealfree { get; set; }
            public IEnumerable<StoreViewModel> StoreList { get; set; }

            public string DealImages { get; set; }
            public string DealImageDesc { get; set; }
            public string ImageTags { get; set; }


            public IEnumerable<HttpPostedFileBase> files { get; set; }

        }


        public class ViewSingleDeal
        {

            public int DealId { get; set; }
            [DisplayName("Expire On")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime ValidTill { get; set; }
            public string DealImages { get; set; }
            [Required]
            public int CouponQty { get; set; }
            public int Discount { get; set; }
            public decimal StrikePrice { get; set; }
            public decimal Price { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public decimal CouponPrice { get; set; }
            public string StoreName { get; set; }
            public string Address { get; set; }
            public bool IsDealFree { get; set; }
        }

        public class CouponList {

            public int CouponId { get; set; }
            public string CouponUniqueText { get; set; }
            public System.DateTime CouponValidTill { get; set; }
            public decimal CouponPrice { get; set; }
            public int UserId { get; set; }
            public int DealId { get; set; }
            public string Title { get; set; }
            public System.DateTime AddedOn { get; set; }
            public int CouponQty { get; set; }
            public int ReedemNo { get; set; }
            public string StoreName { get; set; }
            public string Address { get; set; }

        }


    }
}