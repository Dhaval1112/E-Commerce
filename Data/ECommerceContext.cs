using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class ECommerceContext :IdentityDbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options):base(options)
        {
            
        }

        // this name of the object will be saved as table name in this case Users
        public DbSet<User> Usersdata { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ECommerce;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }*/

    }
}
