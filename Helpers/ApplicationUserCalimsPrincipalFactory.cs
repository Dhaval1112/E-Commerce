using E_Commerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Helpers
{
    public class ApplicationUserCalimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        public ApplicationUserCalimsPrincipalFactory(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options):base(userManager,roleManager,options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity=await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            return identity;
        }
    }
}
