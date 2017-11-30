using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealsNZ.Models.StoreModel;

namespace DealsNZ.Repository.Interface
{
    interface IStore:_IRepositoryList<Store>
    {
        // for store 
        IEnumerable<Store> GetAllStores();
        int CreateStore(Store _store);
        Store GetStoreById(int id);
        void RemoveStorebyId(int id);
        void UpdateStore(Store _store);
        Store GetStoreName(string storeName);     
    }
}
