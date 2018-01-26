using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealsNZ.Models
{
    public class WalletModel
    {
        public class AddMoney
        {
            [Required]
            public int EnterMoney { get; set; }
        }
    }
}