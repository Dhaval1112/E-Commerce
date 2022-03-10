using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public string Pincode { get; set; }

        public string AddressArea { get; set; }

        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(150)]
        public string State { get; set; }
        [MaxLength(500)]
        public string Landmark { get; set; }

        public ApplicationUser user { get; set; }
        public string userId { get; set; }
     
    }
}
