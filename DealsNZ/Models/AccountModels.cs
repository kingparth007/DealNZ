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
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [NotMapped]
            [DataType(DataType.Password)]

            [StringLength(8, ErrorMessage = "Length Between 6 to 8 character", MinimumLength = 6)]
            [RegularExpression("^.*(?=.{6,8})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 8 charachter")]
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
            public string LogInEmail { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [DisplayName("Password")]
            [StringLength(8, ErrorMessage = "Length Between 6 to 8 character", MinimumLength = 6)]
            [RegularExpression("^.*(?=.{6,8})(?=.*[a-zA-Z])(?=.*\\d).*$", ErrorMessage = "Password Contain 1 uppercase 1 lower case and at least 1 number with maximum 8 charachter")]
            public string LogInPassword { get; set; }

        }

    }
}