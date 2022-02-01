using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class SignUpUserModel
    {
        [Required,Display(Name ="First name")]
        public string FirstName { get; set; }
        
        [Required,Display(Name ="Last name")]
        public string LastName { get; set; }

        [Required,DataType(DataType.EmailAddress)]
        [Display(Name ="Email address ")]
        public string Email { get; set; }
/*
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }*//*
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }*/

        [Display(Name ="Mobile number "),Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string MobileNumber { get; set; }


        [Required,DataType(DataType.Password),Compare("ConfirmPassword",ErrorMessage ="Both password and confirm password should be same..!")]
        public string Password { get; set; }


        [Required,DataType(DataType.Password)]
        [Display(Name ="Confirm Password ")]
        public string ConfirmPassword { get; set; }

    }
}
