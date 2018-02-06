using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.Interface
{
    interface IUserWallet : _IRepositoryList<Wallet>
    {
        bool WalletAtRegister(int id);
       string ShowWalletAmount(int UserID);
        bool WalletUpdate(Wallet UpdateWallet);
        Wallet GetCreditByUserID(int UserID);
        Wallet GetCreditByDealUserID(int DealID);
    }
}
