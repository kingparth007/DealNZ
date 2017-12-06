using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Repository.ClassServices
{
    public class DealImageService : Repository<DealImage>, IDealImage
    {
        protected readonly DealsDB DealDb;
        public DealImageService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public int CreateDealImage(DealImage dealImage)
        {
            Insert(dealImage);
            SaveChange();
            var id = dealImage.DealImageId;
            return id;
        }

        public IEnumerable<DealImage> GetDealImageByDealId(int id)
        {
            return Get(x => x.DealId == id);
        }
    }
}