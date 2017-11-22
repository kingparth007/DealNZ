using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.Interface
{
    interface ISubscription : _IRepositoryList<Subscription>
    {
        Subscription GetSubscriptionNameForCheck(string SubscriptionName);
        string UpdateSubscription(Subscription Class);

    }
}
