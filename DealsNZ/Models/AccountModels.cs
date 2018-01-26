using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DealsNZ.Models
{
    public class AccountModels
    {
        public class Register
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            //[RegularExpression("/^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$/", ErrorMessage = "Email id is not Valid")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [DisplayName("Password")]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
            [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            public string Password { get; set; }

            [DataType(DataType.Password)]

            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
            [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            [DisplayName("Confirm Password")]
            [Compare("Password", ErrorMessage = "It should be similar to Password")]
            public string ConformPassword { get; set; }
            [Required]
            public string Name { get; set; }

        }

        public class Login
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [DisplayName("Email")]
            //[RegularExpression("/^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$/", ErrorMessage = "Email id is not Valid")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string LogInEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [DisplayName("Password")]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
           // [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            public string LogInPassword { get; set; }

        }
        public class ForgotPass
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [DisplayName("Email")]
            //[RegularExpression("/^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$/", ErrorMessage = "Email id is not Valid")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string ForgotPassEmail { get; set; }
        }
        public class ResetPass
        {
            [Required]
            public int ForgotUserID { get; set; }


            [Required]
            [DataType(DataType.Password)]
            [DisplayName("Password")]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
        //    [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
           // [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            [DisplayName("Confirm Password")]
            [Compare("Password", ErrorMessage = "It should be similar to Password")]
            public string ConformPassword { get; set; }

        }

        public class ChangePassword
        {
            [Required]
            [DataType(DataType.Password)]
            [DisplayName("Old Password")]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
        //    [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            public string CHOldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [DisplayName("New Password")]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
        //    [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            // [NotEqualTo("OldPassword", "Old Password and New Password are same please change it ")]
            public string CHNewPassword { get; set; }

            [DataType(DataType.Password)]
            [StringLength(10, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 6)]
          //  [RegularExpression("^.*(?=.{6,10})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 10 charachter")]
            [DisplayName("Confirm Password")]
            [Compare("CHNewPassword", ErrorMessage = "It should be similar to New Password")]
            public string CHNewConformPassword { get; set; }
        }

        public class EditProfile
        {

            public int UsrID { get; set; }//readonly
            public string Usrtype { get; set; }//readonly
            public string UserEmail { get; set; }//Readonly
            [DisplayName("Name")]
            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string UserName { get; set; }

            [DisplayName("Number")]
            [RegularExpression("^[0-9]+$", ErrorMessage = "Number does not contain alphabates")]

            [StringLength(20, ErrorMessage = "Length Between 0 to 10 character", MinimumLength = 0)]
            public string UserNumber { get; set; }

            [DisplayName("Unit/Street")]
            public string Street { get; set; }
            // [DisplayName("Name")]
            [StringLength(40, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string Surbrb { get; set; }

            // [DisplayName("Name")]
            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string City { get; set; }

            //[DisplayName("Name")]
            [StringLength(20, ErrorMessage = "Length Between 0 to 20 character", MinimumLength = 0)]
            public string Region { get; set; }

            //[DisplayName("Name")]
            [StringLength(20, ErrorMessage = "Length Between 6 to 10 character", MinimumLength = 0)]
            public string Country { get; set; }

            [DisplayName("ZipCode")]
            [RegularExpression("^[0-9]+$", ErrorMessage = "Number does not contain alphabates")]
            [StringLength(4, ErrorMessage = "Length Between 0 to 4 character", MinimumLength = 0)]
            public string PinCode { get; set; }

        }

    }
}