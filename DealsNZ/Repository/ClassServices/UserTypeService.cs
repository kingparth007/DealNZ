using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserTypeService : Repository<UserType>, IUserType
    {
        protected readonly DealsDB DealDb;
        public UserTypeService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public UserType GetUserByName(string usertypename)
        {
            var usertype = DealDb.UserTypes.Where(x => x.UserTypeName == usertypename).SingleOrDefault();
            return usertype;

        }

        public string UpdateUserType(UserType usertype)
        {
            UserType Update = DealDb.UserTypes.Find(usertype.UserTypeId);
            DealDb.Entry(Update).CurrentValues.SetValues(usertype);
            //DealDb.Entry(usertype).State = EntityState.Modified;
            return "";
        }
    }
}
