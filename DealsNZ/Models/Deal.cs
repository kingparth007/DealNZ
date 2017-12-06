//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DealsNZ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deal()
        {
            this.Coupons = new HashSet<Coupon>();
            this.DealImages = new HashSet<DealImage>();
        }
    
        public int DealId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<System.DateTime> ValidTill { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> StrikePrice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Coupon> Coupons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealImage> DealImages { get; set; }
        public virtual Store Store { get; set; }
    }
}
