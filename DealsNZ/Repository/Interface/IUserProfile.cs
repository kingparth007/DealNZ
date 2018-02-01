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
        UserProfile LoginDetail(Models.AccountModels.Login Login);
        UserVerification ForgotPassUser(string guid);
        bool UserMail(string Body, string subject, string email);
        //  bool UserActivate(string guid);
        bool UpdateUser(UserProfile user);
        bool RemoveLinkForResetPassword(int userid);
        bool IsAuthenticated();
        bool Veryfication(int UserID, string email, string name);
    }
}
