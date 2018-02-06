using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Helpers
{
    public class KeyList
    {
        public class SessionKeys
        {
            public const string UserEmail = "UserEmail";
            public const string UserType = "UserType";
            public const string UserID = "UserID";
            public const string WalletCredit = "WalletCredit";
            public const string AdminEmail = "AdminEmail";
            public const string AdminType = "AdminType";

        }
        public class Users
        {
            public const string Admin = "Admin";
            public const string User = "Users";
            public const string Vendor = "Vendor";
        }

        public class ActivationsKeys
        {
            public const string Register = "Register";
            public const string ResetPass = "ResetPass";
            public const string StoreVerification = "StoreVerification";
            
        }

        public class LogMessages
        {
            public const string LoginMessage = "Register_Login/LoginUser --  User-Logged-IN";
            public const string LogOutMessage = "Register_Login/LoggedOut --  User-Logged-Out";
            public const string ResetPassword = "Register_Login/ResetPassword --  Password reset done by user";
            public const string ChangePassword = "Register_Login/ChagePassword --  Password Changed by user";
            public const string ChangeProfile = "Register_Login/MyProfile --  Profile Updated by User";
            public const string CreateStore = "Admin/CreateStore --  Store created by User";
            public const string EditStore = "Admin/EditStore --  Store Updated by User";
            public const string CreateDeal = "Admin/CreateDeal --  Deal created by User";
            public const string EditDeal = "Admin/EditDeal --  Store created by User";
            public const string ValidateStore = "Admin/Validate --  Store Validate by User";
            public const string DeleteStore = "Admin/DeleteStore --  Store delete setting changed by User";
        }

    }
}