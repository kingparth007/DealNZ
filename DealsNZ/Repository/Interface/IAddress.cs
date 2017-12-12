using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsNZ.Repository.Interface
{
    interface IAddress:_IRepositoryList<Address>
    {
        bool CreateAddress(Address _address);
       
        Address GetAddressById(int id);
        void RemoveAddress(int id);
        void UpdateAddress(Address _address);
        IEnumerable<Address>  GetAddressBystoreId(int id);
    }
}
