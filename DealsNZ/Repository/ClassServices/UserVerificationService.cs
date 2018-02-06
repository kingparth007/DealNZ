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
        IUserProfile UserprofileService;
        public UserVerificationService(DealsDB Data) : base(Data)
        {
            DealDb = Data;

        }

        public bool UserActivate(string guid)
        {
            Guid userGuid = Guid.Parse(guid);
            UserVerification ActivateUser = DealDb.UserVerifications.Where(x => x.UserVerificationCode == userGuid).SingleOrDefault();
            if (ActivateUser != null)
            {

                UserProfile getuserforactivation = DealDb.UserProfiles.Find(ActivateUser.Userid);
                getuserforactivation.isContactVerified = true;
                //var verifiedsucess = UserprofileService.UpdateUser(getuserforactivation);
                UserprofileService = new UserProfileServices(DealDb);
                if (UserprofileService.UpdateUser(getuserforactivation) == true)
                {
                    Delete(ActivateUser);
                    UserprofileService.Dispose();
                    return true;
                }
            }
            return false;
        }
        
    }
}