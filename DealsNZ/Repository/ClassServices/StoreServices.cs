using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System.Linq;

namespace DealsNZ.Repository.ClassServices
{
    public class StoreServices : Repository<Store>, IStore
    {
        
        protected readonly DealsDB DealDb;
        public StoreServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }
        #region Store service
        public int CreateStore(Store store)
        {
            Insert(store);
            SaveChange();
            var id = store.StoreId;
            return id;
        }



      public Store GetStoreName(string storeName)
        { 
            Store name = DealDb.Stores.Where(x => x.StoreName == storeName).SingleOrDefault();
            return name;
        }

        public void RemoveStorebyId(int id)
        {
            Delete(GetByID(id));
            SaveChange();

        }

        public void UpdateStore(Store _store)
        {
            DealDb.Entry(_store).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
            
        }
        #endregion


    }
}