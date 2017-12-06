using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DealsNZ.Repository.Interface
{
    interface IStore:_IRepositoryList<Store>
    {
        // for store 
       
        int CreateStore(Store _store);      
        void RemoveStorebyId(int id);
        void UpdateStore(Store _store);
        Store GetStoreName(string storeName);     
    }
}
