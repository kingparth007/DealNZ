using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class SubscriptionServices : Repository<Subscription>, ISubscription
    {

        protected readonly DealsDB DealDb;
        public SubscriptionServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public Subscription GetSubscriptionNameForCheck(string SubscriptionName)
        {


            Subscription ans = DealDb.Subscriptions.Where(x => x.SubscriptionTitle == SubscriptionName).SingleOrDefault();
            return ans;

        }

        public string UpdateSubscription(Subscription Class)
        {

            Subscription Update = DealDb.Subscriptions.Find(Class.SubscriptionId);
            if (Update != null)
            {
                DealDb.Entry(Update).CurrentValues.SetValues(Class);
                //DealDb.Entry(Class).State = EntityState.Modified;
                DealDb.SaveChanges();
            }

            return "";
        }
    }
}