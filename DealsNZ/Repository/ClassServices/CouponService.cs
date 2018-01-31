using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using DealsNZ.Repository.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Repository.ClassServices
{
    public class CouponService : Repository<Coupon>, ICoupon
    {
        protected readonly DealsDB DealDb;
        public CouponService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public void UpdateCoupon(Coupon _coupon)
        {
            DealDb.Entry(_coupon).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }
    }
}