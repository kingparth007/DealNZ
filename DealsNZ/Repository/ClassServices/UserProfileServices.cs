using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DealsNZ.Models.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserProfileServices : Repository<UserProfile>, IUserProfile
    {
        protected readonly DealsDB DealDb;

        UnitOfWorks unitofworks = new UnitOfWorks(new DealsDB());

        public UserProfileServices(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }


        public UserProfile GetUserByEmail(string Email)
        {
            return DealDb.UserProfiles.Where(x => x.Email.ToLower().ToString() == Email.ToLower().ToString()).SingleOrDefault();
        }
        public bool CheckEmail(string Email)
        {
            if (DealDb.UserProfiles.Where(x => x.Email.ToLower().ToString() == Email.ToLower().ToString()).Count() == 0)
            {
                return true;
            }
            return false;
        }
        public bool LoginDetail(Models.AccountModels.Login Login)
        {

            if (DealDb.UserProfiles.Where(x => x.Email.ToLower().ToString() == Login.LogInEmail.ToLower().ToString() && Decryptdata(x.Password) == Login.LogInPassword).Count() == 1)
            {
                return true;
            }
            return false;
        }
        public bool RegisterUser(Models.AccountModels.Register Register)
        {


            try
            {
                UserProfile InsertUser = new UserProfile()
                {
                    UserType = 5,
                    Name = Register.Name,
                    Email = Register.Email,
                    Password = PasswordEncrypt(Register.Password),
                    AddedOn = System.DateTime.Now.Date,
                    isContactVerified = false
                };
                Insert(InsertUser);
                SaveChange();
                int id = InsertUser.UserId;
                //if (unitofworks.UserWallet.WalletAtRegister(id) == true)
                //{
                //    UserVerification UserVerificationAtRegister = new UserVerification()
                //    {
                //        Userid = id,
                //        Purpose = "Register",
                //        AddedOn = System.DateTime.Now.Date
                //    };

                //    if (UserVerificationAtRegister != null)
                //    {
                //        unitofworks.UserVerification.Insert(UserVerificationAtRegister);
                //        SaveChange();
                //        Guid verificationId = UserVerificationAtRegister.UserVerificationID;

                //        return true;
                //    }

                //    return false;
                //}
                return true;

            }
            catch
            {
                return false;
            }

        }

        public string PasswordEncrypt(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encode); ;
        }
        public string Decryptdata(string encryptpwd)
        {
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            return decoded_char.ToString();
        }
    }
}