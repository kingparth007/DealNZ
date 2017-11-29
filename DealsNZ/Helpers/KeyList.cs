﻿using System;
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
            public const string UserID = "UserEmail";
            
            public const string AdminEmail = "AdminEmail";
            public const string AdminType = "AdminType";

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
        }

    }
}