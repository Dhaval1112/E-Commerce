using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Cart> Carts { get; set; }

    }
}
