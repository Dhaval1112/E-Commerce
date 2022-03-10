using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{

    public class AddressModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Mobile number "), Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string MobileNumber { get; set; }

       // [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$",ErrorMessage ="Please enter valid pincode")]
        public string Pincode { get; set; }

       
        public string AddressArea { get; set; }

        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(150)]
        public string State { get; set; }
        [MaxLength(500)]
        public string Landmark{ get; set; }

    }
}
