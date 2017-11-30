using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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

        //UnitOfWorks unitofworks = new UnitOfWorks(new DealsDB());
        IUserWallet WalletService;
        IUserVerification VerificationService;

        public object MessageBox { get; private set; }

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
        public UserProfile LoginDetail(Models.AccountModels.Login Login)
        {
            string encpass = PasswordEncrypt(Login.LogInPassword);
            var loggeduser = DealDb.UserProfiles.Where(x => x.Email.Equals(Login.LogInEmail) && x.Password == encpass).SingleOrDefault();
            return loggeduser;
        }
        public bool RegisterUser(Models.AccountModels.Register Register)
        {
            try
            {
                var usertype = DealDb.UserTypes.Where(x => x.UserTypeName.Equals("Users")).SingleOrDefault();
                UserProfile InsertUser = new UserProfile()
                {
                    UserType = usertype.UserTypeId,
                    Name = Register.Name,
                    Email = Register.Email.ToLower().ToString(),
                    Password = PasswordEncrypt(Register.Password),
                    AddedOn = System.DateTime.Now.Date,
                    isContactVerified = false
                };
                string dc = Decryptdata(PasswordEncrypt(Register.Password));
                Insert(InsertUser);
                SaveChange();
                int id = InsertUser.UserId;
                WalletService = new UserWalletServices(DealDb);
                if (WalletService.WalletAtRegister(id) == true)
                {
                    string Purpose = "Register";
                    Guid guid = Guid.NewGuid();
                    UserVerification UserVerificationAtRegister = new UserVerification()
                    {
                        UserVerificationCode = guid,
                        Userid = id,
                        Purpose = Purpose,
                        AddedOn = System.DateTime.Now.Date
                    };
                    VerificationService = new UserVerificationService(DealDb);
                    if (UserVerificationAtRegister != null)
                    {
                        VerificationService.Insert(UserVerificationAtRegister);
                        //  VerificationService.SaveChange();
                        VerificationService.Dispose();
                        WalletService.Dispose();
                        //Mail Logic
                        // string enc = PasswordEncrypt(UserVerificationAtRegister.UserVerificationCode + "|" + UserVerificationAtRegister.Purpose);
                        string URL = "http://localhost:20629/Activation/Activate/" + UserVerificationAtRegister.UserVerificationCode;
                        //string dec = Decryptdata(enc);
                        UserMail(URL, "Activate Your Account", InsertUser.Name, InsertUser.Email);
                        return true;
                    }
                    WalletService.Dispose();
                    return false;
                }
                WalletService.Dispose();
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool UserMail(string uRL, string subject, string name, string email)
        {
            string Hostemail = ConfigurationManager.AppSettings["HostMail"];
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            MailMessage ContectMail = new MailMessage(Hostemail, email)
            {
                Body = uRL,
                IsBodyHtml = true,
                Subject = subject
            };
            SmtpClient SMTP = new SmtpClient()
            {
                Credentials = new System.Net.NetworkCredential(username, password)
            };
            //try
            //{
            SMTP.Send(ContectMail);
            return true;
            //}
            //catch
            //{
            //    return false;
            //}


            //try
            //{
            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //mail.From = new MailAddress("parthking02@gmail.com");
            //mail.To.Add(email);
            //mail.Subject = subject;
            //mail.Body = uRL;  //"This is for testing SMTP mail from GMAIL";

            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("parthking02", "asdfghjkl20");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);
            //return true;
            ////  MessageBox.Show("mail Send");
            ////}
            ////catch (Exception ex)
            ////{
            ////    //MessageBox.Show(ex.ToString());
            ////    return false;
            ////}

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
            string[] getstring = new string[charCount];
            string DecryptString = String.Join("", decoded_char);
            return DecryptString;

        }

        public UserVerification ForgotPassUser(string guid)
        {

            UserVerification user = DealDb.UserVerifications.Where(x => x.UserVerificationCode.ToString().Equals(guid)).SingleOrDefault();

            return user;
        }
        public bool UpdateUser(UserProfile user)
        {
            UserProfile MatchUser = DealDb.UserProfiles.Find(user.UserId);
            if (MatchUser != null)
            {
                // try
                // {
                DealDb.Entry(MatchUser).CurrentValues.SetValues(user);
                DealDb.SaveChanges();
                return true;
                //}
                //catch
                //{ return false; }
            }
            return false;
        }

        public bool RemoveLinkForResetPassword(int userid)
        {

            VerificationService = new UserVerificationService(DealDb);
            UserVerification RemoveLink = DealDb.UserVerifications.Where(x => x.Userid.Equals(userid) && x.Purpose.Equals("ResetPass")).SingleOrDefault();
            VerificationService.Delete(RemoveLink);
            VerificationService.Dispose();
            return true;
        }
    }
}