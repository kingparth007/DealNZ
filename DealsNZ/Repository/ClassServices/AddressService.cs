using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Repository.ClassServices
{
    public class AddressService : Repository<Address>,IAddress
    {
       
        protected readonly DealsDB DealDb;
        public AddressService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public bool CreateAddress(Address _address)
        {
            Insert(_address);
            SaveChange();
            return true;
        }

        public Address GetAddressById(int id)
        {
          return  GetByID(id);

        }

       
      
        public void RemoveAddress(int id)
        {
            Delete(GetAddressById(id));
            
        }

        public void UpdateAddress(Address _address)
        {
            DealDb.Entry(_address).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }

        public IEnumerable<Address> GetAddressBystoreId(int id)
        {
            return Get(x => x.StoreId == id);
     
        }
    }
}