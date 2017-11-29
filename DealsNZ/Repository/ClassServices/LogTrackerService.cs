using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Repository.ClassServices
{
    public class LogTrackerService : Repository<LogTracker>, ILogTracker
    {
        protected readonly DealsDB DealDb;
        public LogTrackerService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

    }
}