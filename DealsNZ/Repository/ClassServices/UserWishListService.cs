using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Repository.ClassServices
{
    public class UserWishListService : Repository<WishList>,IUserWishList
    {
        protected readonly DealsDB DealDb;
        public UserWishListService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public IEnumerable<DealsModels.DealViewModel> ViewWishListByUser(int UserID) {

            var DealWishList = GetAll().Where(x => x.UserId == UserID);
            
            List<DealsModels.DealViewModel> Wish = new List<DealsModels.DealViewModel>();
            foreach (var item in DealWishList)
            {
                Models.DealsModels.DealViewModel ds = new DealsModels.DealViewModel();
                ds.DealId = item.DealId;
                ds.DealImages = (item.Deal.DealImages.Select(x => new { x.DealImage1 }).FirstOrDefault()).DealImage1;
                ds.Price = Convert.ToInt32(item.Deal.Price);
                ds.Description = item.Deal.Description;
                ds.Discount = Convert.ToInt32(item.Deal.Discount);
                ds.ValidTill = DateTime.Parse(item.Deal.ValidTill.ToString());
                ds.IsDealfree = Convert.ToBoolean(item.Deal.IsDealFree);
                ds.IsDeleted = Convert.ToBoolean(item.Deal.IsDeleted);
                ds.Title = item.Deal.Title;
                ds.StrikePrice = Convert.ToInt32(item.Deal.StrikePrice);
                Wish.Add(ds);
            }

            return Wish;
        }

        public bool wishlistCheck(int DealId, int UserID) {

            int CheckWishList = DealDb.WishLists.Where(x => x.DealId == DealId && x.UserId == UserID).Count();

            if (CheckWishList == 0) {

                return true;
            }
            return false;

        }
    }
}