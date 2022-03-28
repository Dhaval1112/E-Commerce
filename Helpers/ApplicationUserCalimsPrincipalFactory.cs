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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ApplicationUserCalimsPrincipalFactory(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options):base(userManager,roleManager,options)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity=await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            
            var roles =await UserManager.GetRolesAsync(user);

            //TODO: ROLE MANAGEMENT
            identity.AddClaim(new Claim("UserRoles", roles.Contains("Admin")?"Admin":"User"));
            //identity.AddClaim(new Claim("UserRole", await));
            return identity;
        }
    }
}
