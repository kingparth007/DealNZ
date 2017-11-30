using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealsNZ.Models
{
    public class StoreModel
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

        
    }
}