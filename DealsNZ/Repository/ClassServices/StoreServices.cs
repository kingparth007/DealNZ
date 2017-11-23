using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Repository.ClassServices
{
    public class StoreServices : Repository<Store>, IStore
    {
        protected readonly DealsDB DealDb;
        public StoreServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public int CreateStore(Store _store)
        {
            var ids = GetAllStores();
            foreach (var isd in ids)
            {if(isd==store.StoreId)
                Insert(_store);
                SaveChange();
                var id = _store.StoreId;
                return id;
            }

        }

        public IEnumerable<Store> GetAllStores()
        {
            var storeList = GetAll();
            return storeList;
        }

        public Store GetStoreById(int id)
        {
            return GetByID(id);

        }

        public void RemoveStorebyId(int id)
        {
            Delete(GetByID(id));

        }

        public void updateStore(Store _store)
        {
            DealDb.Entry(_store).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }
    }
}