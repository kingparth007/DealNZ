using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;


namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserSubscriptionServices : Repository<UserSubscrition>, IUserSubscription
    {
        protected readonly DealsDB DealDb;
        public UserSubscriptionServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public bool UserSubscriptionAtRegister(int id, int sub_id)
        {
            UserSubscrition InsertUserSubscriptionAtStoreRegister = new UserSubscrition()
            {
                UserId = id,
                SubscriptionId = 1,
                UserSubscritionDate = System.DateTime.Now.Date


            };
            if (InsertUserSubscriptionAtStoreRegister != null)
            {
                Insert(InsertUserSubscriptionAtStoreRegister);
                SaveChange();
                return true;
            }

            return false;
        }
    }
}