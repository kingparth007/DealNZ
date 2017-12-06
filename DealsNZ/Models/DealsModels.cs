using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
            public string CompanyName { get; set; }
            public IEnumerable<CompanyViewModel> CompanyList { get; set; }

            //for address
            public string Street { get; set; }
            public string City { get; set; }
            public string Country { get; set; }

            public HttpPostedFileBase Image { get; set; }
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
            public DateTime ValidTill { get; set; }
            public int Discount { get; set; }
            public int StrikePrice { get; set; }
            public int Price { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime AddedOn { get; set; }
            public bool IsDeleted { get; set; }
            public int StoreId { get; set; }

            public IEnumerable<StoreViewModel> StoreList { get; set; }

            public string DealImages { get; set; }
            public string DealImageDesc { get; set; }
            public string ImageTags { get; set; }

            public virtual ICollection<DealImage> DealImagesList { get; set; }

        }


    }
}