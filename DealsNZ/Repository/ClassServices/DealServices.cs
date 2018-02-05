using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DealsNZ.Models.DealsModels;

namespace DealsNZ.Repository.ClassServices
{
    public class DealServices : Repository<Deal>, IDeal
    {
        protected readonly DealsDB DealDb;

        public DealServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }
        
        public IEnumerable< DealsModels.DealViewModel> AllDeal()
        {
            var deallist = DealDb.Deals.Select(x => new { x.Title,x.StrikePrice,x.IsDealFree, Id = x.DealId,expire=x.ValidTill, description = x.Description, price = x.Price, discount = x.Discount,IsDeleted=x.IsDeleted ,DealImages = x.DealImages.FirstOrDefault()}).ToList();
          
            List<DealsModels.DealViewModel> d = new List<DealsModels.DealViewModel>();
            foreach (var item in deallist)
            {
                Models.DealsModels.DealViewModel ds = new DealsModels.DealViewModel();
                ds.DealId = item.Id;
                ds.DealImages = item.DealImages.DealImage1;
                ds.Price =Convert.ToInt32(item.price);
                ds.Description = item.description;
                ds.Discount = Convert.ToInt32(item.discount);
                ds.ValidTill = DateTime.Parse( item.expire.ToString());
                ds.IsDealfree =Convert.ToBoolean( item.IsDealFree);
                ds.IsDeleted = Convert.ToBoolean(item.IsDeleted);
                ds.Title = item.Title;
                ds.StrikePrice = Convert.ToInt32(item.StrikePrice);
                d.Add(ds);
            }

            return d ;
        }

        public ViewSingleDeal GetSingleDeal(int Id) {

            Deal getDeal = GetByID(Id);
            ViewSingleDeal SingleDeal = new ViewSingleDeal();
            SingleDeal.DealId = getDeal.DealId;
            SingleDeal.DealImages = getDeal.DealImages.FirstOrDefault().DealImage1;
            SingleDeal.Price = Convert.ToInt32(getDeal.Price);
            SingleDeal.Description = getDeal.Description;
            SingleDeal.Discount = Convert.ToInt32(getDeal.Discount);
            SingleDeal.ValidTill = DateTime.Parse(getDeal.ValidTill.ToString());
            SingleDeal.Title = getDeal.Title;
            SingleDeal.StrikePrice = Convert.ToInt32(getDeal.StrikePrice);
            SingleDeal.CouponPrice = Convert.ToInt32(getDeal.StrikePrice);
            SingleDeal.Address = getDeal.Store.Addresses.FirstOrDefault().Street.ToString() + " " + getDeal.Store.Addresses.FirstOrDefault().City.ToString() + " " + getDeal.Store.Addresses.FirstOrDefault().Country.ToString();
            SingleDeal.StoreName = getDeal.Store.StoreName;
            SingleDeal.IsDealFree = Convert.ToBoolean(getDeal.IsDealFree);

            return SingleDeal;
        } 

        public int CreateDeal(Deal deal)
        {
            Insert(deal);
            SaveChange();
            var id = deal.DealId;
            return id;
        }



        public void RemoveDealbyId(int id)
        {
            Delete(GetByID(id));
            SaveChange();
        }

        public void UpdateDeal(Deal deal)
        {
            DealDb.Entry(deal).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }
    }
}