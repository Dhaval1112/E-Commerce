using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class SignUpUserModel
    {
        [Required,DataType(DataType.EmailAddress)]
        [Display(Name ="Email address ")]
        public string Email { get; set; }
          
        [Required,DataType(DataType.Password),Compare("ConfirmPassword",ErrorMessage ="Both password and confirm password should be same..!")]
        public string Password { get; set; }

        [Required,DataType(DataType.Password)]
        [Display(Name ="Confirm Password ")]
        public string ConfirmPassword { get; set; } 
    }
}
