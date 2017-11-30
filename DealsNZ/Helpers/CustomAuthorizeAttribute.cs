﻿using DealsNZ.Models;
using DealsNZ.Models.Repository.ClassServices;
using DealsNZ.Models.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealsNZ.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        IUserProfile userProfileService = new UserProfileServices(new DealsDB());
        IUserType userTypeService = new UserTypeService(new DealsDB());
        protected readonly DealsDB DealDb;
        private readonly string[] allowedroles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userSessionId = HttpContext.Current.Session[KeyList.SessionKeys.UserID].ToString();

            var user = userProfileService.GetByID(Convert.ToInt32(userSessionId));
            foreach (var role in allowedroles)
            {
                if (user!=null && user.UserType1.UserTypeName.Contains(role))

                {
                    authorize = true; /* return true if Entity has current user(active) with specific role */
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
