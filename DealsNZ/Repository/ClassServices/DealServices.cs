using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            var deallist = DealDb.Deals.Select(x => new { Id = x.DealId,expire=x.ValidTill, description = x.Description, price = x.Price, discount = x.Discount, DealImages = x.DealImages.FirstOrDefault()}).ToList();
           // var deallist = DealDb.Deals.Select(x => new { id = x.DealId,price=x.Price,expire=x.ValidTill, discount=x.Discount,description=x.Description, DealImages = DealDb.DealImages.Where(a => a.DealId == x.DealId).Select(a => new { a.DealImage1 }).FirstOrDefault() }).ToList();
            List<DealsModels.DealViewModel> d = new List<DealsModels.DealViewModel>();
            foreach (var item in deallist)
            {
                Models.DealsModels.DealViewModel ds = new DealsModels.DealViewModel();
                ds.DealId = item.Id;
                ds.DealImages = item.DealImages.DealImage1;
               ds.Price =Convert.ToInt32(item.price);
                ds.Description = item.description;
                ds.Discount = Convert.ToInt32(item.discount);
                ds.ValidTill = DateTime.Parse(item.expire.ToString());
                 // DateTime.Parse(item.expire.ToString()).ToShortDateString().ToString();

                d.Add(ds);
            }

            return d ;
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