using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.Interface
{
    interface IUserSubscription : _IRepositoryList<UserSubscrition>
    {
        bool UserSubscriptionAtRegister(int id, int sub_id);
    }

}
