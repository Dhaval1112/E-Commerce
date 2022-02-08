using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class UserProfileModel
    {
      
        [Required, Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name ="Last Name")]
        public string LastName { get; set; }
        /*[Required, Display(Name ="Gender")]*/
        
        public string Gender { get; set; }
        [Display(Name ="Mobile Number")]
        
        public string MobileNumber { get; set; }
        

        public string EmailAddress { get; set; }
        [Required, Display(Name ="Date Of Birth"), DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public bool IsSuccess { get; set; }
       
    }
}
