using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsNZ.Repository.Interface
{
    interface ICoupon : _IRepositoryList<Coupon>
    {

        void UpdateCoupon(Coupon _coupon);
        IEnumerable<DealsModels.CouponList> ViewCoupons(int UserID);
    }
}
