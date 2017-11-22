using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserWalletServices : Repository<Wallet>, IUserWallet
    {
        protected readonly DealsDB DealDb;
        public UserWalletServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }
    }
}