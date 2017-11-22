using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.Interface
{
    interface IUserProfile : _IRepositoryList<UserProfile>
    {

        bool CheckEmail(string Email);
        bool RegisterUser(Models.AccountModels.Register Register);
        string PasswordEncrypt(string password);
        string Decryptdata(string encryptpwd);
        UserProfile GetUserByEmail(string Email);
        bool LoginDetail(Models.AccountModels.Login Login);
    }
}
