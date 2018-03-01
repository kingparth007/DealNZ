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


        //Instert Default Data in wallet at Register Time
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

        // To get user wallet amount for transaction like add money or update money
        public Wallet GetCreditByUserID(int UserID)
        {

            Wallet WalletAmount = DealDb.Wallets.Where(x => x.UserId == UserID).OrderByDescending(x => x.WalletCreditDate).FirstOrDefault();
            
            return WalletAmount;

        }

        //To Get Deal user wallet amount for update when user create coupon
        public Wallet GetCreditByDealUserID(int DealID)
        {

            var CreditUser = DealDb.Deals.Where(x => x.DealId == DealID).SingleOrDefault();

            Wallet WalletAmount = DealDb.Wallets.Where(x => x.UserId == CreditUser.Store.UserId).OrderByDescending(x => x.WalletCreditDate).FirstOrDefault();
            
            return WalletAmount;

        }

        // To show wallet amount when user Logged in 
        public string ShowWalletAmount(int UserID)
        {

            var WalletAmount = DealDb.Wallets.Where(x => x.UserId == UserID).OrderByDescending(x => x.WalletCreditDate).FirstOrDefault();


            return WalletAmount.WalletCredit.ToString();
        }

        //Wallet Update Function
        public bool WalletUpdate(Wallet UpdateWallet)
        {

            Wallet MatchWallet = DealDb.Wallets.Where(x => x.UserId == UpdateWallet.UserId).FirstOrDefault();
            if (MatchWallet != null)
            {

                DealDb.Entry(MatchWallet).CurrentValues.SetValues(UpdateWallet);
                DealDb.SaveChanges();
                return true;
            }
            return false;
        }
    }
}