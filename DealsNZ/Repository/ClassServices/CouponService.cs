using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using DealsNZ.Repository.Interface;
using DealsNZ.Models;
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


        public IEnumerable<DealsModels.CouponList> ViewCoupons(int UserID)
        {
            var ViewCoupons = DealDb.Coupons.Where(x => x.UserId == UserID).ToList().OrderByDescending(x=>x.CouponId);

            List<DealsModels.CouponList> CouponList = new List<DealsModels.CouponList>();
            foreach (var coupon in ViewCoupons)
            {
                DealsModels.CouponList Cpns = new DealsModels.CouponList()
                {
                    CouponId = coupon.CouponId,
                    CouponPrice = Convert.ToDecimal(coupon.CouponPrice),
                    CouponQty = Convert.ToInt32(coupon.CouponQty),
                    CouponUniqueText = coupon.CouponUniqueText,
                    CouponValidTill = Convert.ToDateTime(coupon.CouponValidTill),
                    DealId = Convert.ToInt32(coupon.DealId),
                    Title = coupon.Deal.Title,
                    UserId = Convert.ToInt32(coupon.UserId),
                    StoreName = coupon.Deal.Store.StoreName,
                    Address = coupon.Deal.Store.Addresses.FirstOrDefault().Street.ToString() + " " + coupon.Deal.Store.Addresses.FirstOrDefault().City.ToString() + " " + coupon.Deal.Store.Addresses.FirstOrDefault().Country.ToString()
                };
                CouponList.Add(Cpns);
            }

            return CouponList;
        }
        public void UpdateCoupon(Coupon _coupon)
        {
            DealDb.Entry(_coupon).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }
    }
}