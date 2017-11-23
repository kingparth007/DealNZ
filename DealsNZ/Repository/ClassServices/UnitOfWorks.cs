using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DealsNZ.Models;
using DealsNZ.Models.Repository.Interface;
using DealsNZ.Models.Repository.ClassServices;

namespace RepoPattern.Models.RepositoryFiles
{
    public class UnitOfWorks : IUnitOFWorks
    {

        public readonly DealsDB _dbcontext;
        internal ISubscription Subscription { get; private set; }
        internal IUserType Usertype { get; private set; }
        internal IUserProfile UserProfile { get; private set; }
        internal IUserSubscription UserSubscription { get; private set; }
        internal IUserVerification UserVerification { get; private set; }
        internal IUserWallet UserWallet { get; private set; }
        public UnitOfWorks(DealsDB _dbc)
        {

            _dbcontext = _dbc;

        }


        public int Complete()
        {
            return _dbcontext.SaveChanges();
        }

        public void Dispose()
        {


            throw new NotImplementedException();
        }
    }
}