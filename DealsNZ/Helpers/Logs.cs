using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsNZ.Models;
using DealsNZ.Repository.ClassServices;
using DealsNZ.Repository.Interface;

namespace DealsNZ.Controllers
{
    public class Logs
    {

        ILogTracker LogService;
        public bool CreateLog(int UserId, string Message)
        {
            LogService = new LogTrackerService(new DealsDB());
            LogTracker InsertLogs = new LogTracker()
            {
                UserId = UserId,
                Message = Message,
                AddedOn = System.DateTime.Now
            };
            if (LogService != null)
            {
                LogService.Insert(InsertLogs);
                LogService.Dispose();
                return true;
            }
            return false;
        }
    }

}