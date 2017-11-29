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

        public bool WalletAtRegister(int id)
        {
            try
            {
                Wallet RegisgterWallet = new Wallet()
                {
                    UserId = id,
                    WalletCredit = 0
                };
                if (RegisgterWallet != null)
                {
                    Insert(RegisgterWallet);
                    SaveChange();
                    return true;
                }
            }
            catch { return false; }
            return false;
        }
    }
}