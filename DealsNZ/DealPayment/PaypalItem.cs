using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.DealPayment
{
    public class PaypalItem
    {
        public int ProductId { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string currency { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
    }
}