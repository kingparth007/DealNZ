using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserVerificationService : Repository<UserVerification>, IUserVerification
    {
        protected readonly DealsDB DealDb;
        public UserVerificationService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

    }
}