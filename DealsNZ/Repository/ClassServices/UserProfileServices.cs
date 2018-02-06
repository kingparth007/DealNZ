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
using DealsNZ.Helpers;
using System.Net.Mime;
using System.IO;

namespace DealsNZ.Models.Repository.ClassServices
{
    public class UserProfileServices : Repository<UserProfile>, IUserProfile
    {
        protected readonly DealsDB DealDb;

        //UnitOfWorks unitofworks = new UnitOfWorks(new DealsDB());
        IUserWallet WalletService;
        IUserVerification VerificationService;

        string Domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
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


            //var a = GetAll().Select(x => new { x.Name, image = DealDb.DealImages.Where(a => a.DealId.Equals(x.UserId)).ToString() }).ToList();
            string encpass = PasswordEncrypt(Login.LogInPassword);
            var loggeduser = DealDb.UserProfiles.Where(x => x.Email.Equals(Login.LogInEmail) && x.Password == encpass).SingleOrDefault();

           
          //  KeyList.SessionKeys.WalletCredit = wallet;
            return loggeduser;
        }
        public bool RegisterUser(Models.AccountModels.Register Register)
        {
            int id = 0;
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
                id = InsertUser.UserId;
                WalletService = new UserWalletServices(DealDb);
                if (WalletService.WalletAtRegister(id) == true)
                {
                    var VerificationMail = Veryfication(id, InsertUser.Email, InsertUser.Name);

                    if (VerificationMail == true)
                    {
                        WalletService.Dispose();
                        return true;
                    }
                    var WalletData = WalletService.GetByID(id);
                    WalletService.Delete(WalletData);
                    var RemoveUser = GetByID(id);
                    Delete(RemoveUser);
                    return false;

                   
                    //string Purpose = "Register";
                    //Guid guid = Guid.NewGuid();
                    //UserVerification UserVerificationAtRegister = new UserVerification()
                    //{
                    //    UserVerificationCode = guid,
                    //    Userid = id,
                    //    Purpose = Purpose,
                    //    AddedOn = System.DateTime.Now.Date
                    //};
                    //VerificationService = new UserVerificationService(DealDb);
                    //if (UserVerificationAtRegister != null)
                    //{
                    //    VerificationService.Insert(UserVerificationAtRegister);
                    //    //  VerificationService.SaveChange();
                    //    VerificationService.Dispose();
                    //    WalletService.Dispose();
                    //    //Mail Logic
                    //    // string enc = PasswordEncrypt(UserVerificationAtRegister.UserVerificationCode + "|" + UserVerificationAtRegister.Purpose);
                    //    string URL = "http://localhost:20629/Activation/Activate/" + UserVerificationAtRegister.UserVerificationCode;
                    //    //string dec = Decryptdata(enc);
                    //    UserMail(URL, "Activate Your Account", InsertUser.Name, InsertUser.Email);
                    //    return true;
                    //}
                    
                }
                WalletService.Dispose();
                return false;
            }
            catch
            {
                var RemoveUser = GetByID(id);
                Delete(RemoveUser);
                return false;
            }
        }

        public bool UserMail(string Body, string subject, string email)
        {
            string Hostemail = ConfigurationManager.AppSettings["HostMail"];
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            MailMessage ContectMail = new MailMessage(Hostemail, email)
            {
                //Body = Body,
                IsBodyHtml = true,
                Subject = subject,

            };
            
            AlternateView HtmlView = AlternateView.CreateAlternateViewFromString(Body, new ContentType("text/html"));
            ContectMail.AlternateViews.Add(HtmlView);

            SmtpClient SMTP = new SmtpClient()
            {
                Credentials = new System.Net.NetworkCredential(username, password)
            };
            //try
            //{
            SMTP.Send(ContectMail);
            return true;
            

        }

        public bool Veryfication(int UserID, string email, string name)
        {

            UserVerification userverify = DealDb.UserVerifications.Where(x => x.Userid.Equals(UserID)).Where(x => x.Purpose.Equals(KeyList.ActivationsKeys.Register)).SingleOrDefault();
            if (userverify != null)
            {
                
                string body = string.Empty;
                //using streamreader for reading my htmltemplate   
                using (StreamReader reader = new StreamReader(@"~/EmailTemp/ActivationLinkTemplate.html"))

                {

                    body = reader.ReadToEnd();

                }


                string URL = Domain + "/Activation/Activate/" + userverify.UserVerificationCode;
                body = body.Replace("{LinkUrl}", URL); //replacing the required things  
                
                UserMail(body, "Activate Your Account", email);
               
                return true;
            }
            else
            {
                string Purpose = "Register";
                Guid guid = Guid.NewGuid();
                UserVerification UserVerificationAtRegister = new UserVerification()
                {
                    UserVerificationCode = guid,
                    Userid = UserID,
                    Purpose = Purpose,
                    AddedOn = System.DateTime.Now.Date
                };
                VerificationService = new UserVerificationService(DealDb);
                if (UserVerificationAtRegister != null)
                {
                    try
                    {
                        VerificationService.Insert(UserVerificationAtRegister);
                    }
                    catch
                    {

                    }

                    //  VerificationService.SaveChange();
                    VerificationService.Dispose();

                    //Mail Logic
                    // string enc = PasswordEncrypt(UserVerificationAtRegister.UserVerificationCode + "|" + UserVerificationAtRegister.Purpose);
                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    using (StreamReader reader = new StreamReader(@"~/EmailTemp/ActivationLinkTemplate.html"))

                    {

                        body = reader.ReadToEnd();

                    }
                    string URL = Domain + "/Activation/Activate/" + UserVerificationAtRegister.UserVerificationCode;
                    body = body.Replace("{LinkUrl}", URL); //replacing the required things  
                    
                    //string dec = Decryptdata(enc);
                    UserMail(body, "Activate Your Account", email);
                    return true;
                }

            }
            return false;
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
        public bool GuidSend(int userid)
        {

            return false;
        }

        public bool IsAuthenticated()
        {
            var session = HttpContext.Current.Session[KeyList.SessionKeys.UserID].ToString();
            if (session != null)
            {
                return true;

            }
            return false;
        }

       
    }
}