using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class AccountRepository : IAccountRepository
    {

        // for adding dependancy objects
        private readonly ECommerceContext _eCommerceContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public AccountRepository(ECommerceContext eCommerceContext, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IUserService userService)
        {
            this._eCommerceContext = eCommerceContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._userService = userService;
        }

        // my normal checking ef 
        public async Task<int> AddNewUserAsync(LoginModel model)
        {
            var user = new User
            {
                Email = model.Email,
                Password = model.Password
            };

            await _eCommerceContext.Usersdata.AddAsync(user);
            int id = await _eCommerceContext.SaveChangesAsync();

            return id;
        }

        //for signup user (means creating user)
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                MobileNumber=userModel.MobileNumber,
                FirstName=userModel.FirstName,
                LastName=userModel.LastName,

            };

            var result=await _userManager.CreateAsync(user,userModel.Password);

            return result;
        }

        // Login into account with password
        public async Task<SignInResult> PasswordSignInAsync(LoginModel loginModel)
        {
            return await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);

        }

        //For logout sign in Users
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(_userService.GetUserId());
            return await _userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);

        }


    }
}
