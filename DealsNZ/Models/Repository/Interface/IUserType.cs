using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.Interface
{
    interface IUserType : _IRepositoryList<UserType>
    {
        UserType GetUserByName(string usertypename);
        string UpdateUserType(UserType usertype);
    }
}
