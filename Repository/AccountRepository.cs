using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;




        public AccountRepository(ECommerceContext eCommerceContext, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IUserService userService,IEmailService emailService,IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this._eCommerceContext = eCommerceContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._userService = userService;
            this._emailService = emailService;
            this._configuration = configuration;
            this._roleManager = roleManager;
        }

        
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        // Get user by id 
        public async Task<UserProfileModel> GetLogedInUser()
        {
            var user =await GetCurrentUser();
            
            UserProfileModel model = new UserProfileModel()
            {
                FirstName = user.FirstName,
                LastName=user.LastName,
                DateOfBirth=user.DateOfBirth,
                EmailAddress=user.Email,
                Gender=user.Gender,
                MobileNumber=user.MobileNumber
            };
            return model;
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

            
            if (result.Succeeded)
            {
                var resultrole=await _userManager.AddToRoleAsync(user, "User");
                await GenerateEmailConfirmationTokenAsync(user);
            }

            return result;
        }
        
        // get user by it's email address
        public async Task<ApplicationUser> GetUSerByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);

        }


        // For getting role
        public async Task<IList<string>> GetRoleByEmail(string email)
        { 
            var user = await _userManager.FindByEmailAsync(email);

            var role = await _userManager.GetRolesAsync(user);
            return role;
        }


        // for regenerating out token and send that to user so that he/she can confirm their email account
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendingMailForConfirmationEmail(user, token);
            }
        }


        public async Task<IdentityResult> UpdateProfile(UserProfileModel user)
        {
            var oldUser = await GetCurrentUser();
            oldUser.DateOfBirth = user.DateOfBirth;
            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            var result =await _userManager.UpdateAsync(oldUser);
            
            return result;
            
        }

        // Generate forgot password async will generate token for forgot passwod and threw it we can recover password
        public async Task GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgetPasswordEmail(user, token);
            }
        }

        // for sending mail For reseting password email
        private async Task SendForgetPasswordEmail(ApplicationUser applicationUser, string token)
        {

            string appDomail = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions userEmailOptions = new UserEmailOptions()
            {
                Subject = "Hello {{UserName}}, Regarding reseting your password",
                ToEmail = applicationUser.Email,
                Heading="Forgot password",
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",applicationUser.FirstName),
                    new KeyValuePair<string, string>
                    ("{{Link}}",String.Format(appDomail+confirmationLink,applicationUser.Id,token)),
                },
                TemplateName = "ForgotPassword"
            };


            await _emailService.sendTestEmail(userEmailOptions);
        }







        // for sending mail to client for confirming email address
        private async Task SendingMailForConfirmationEmail(ApplicationUser applicationUser, string token)
        {

            string appDomail = _configuration.GetSection("Application:AppDomain").Value;
            string forgotpasswordLink = _configuration.GetSection("Application:EmailConfirm").Value;

            UserEmailOptions userEmailOptions = new UserEmailOptions()
            {
                Subject = "Hello {{UserName}}, please confirm you email id" ,
                Heading="Confirm your email",
                ToEmail = applicationUser.Email,
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",applicationUser.FirstName),
                    new KeyValuePair<string, string>
                    ("{{Link}}",String.Format(appDomail+forgotpasswordLink,applicationUser.Id,token)),
                },
                TemplateName= "EmailConfirm"
            };


            await _emailService.sendTestEmail(userEmailOptions);
        }

        // Login into account with password
        public async Task<SignInResult> PasswordSignInAsync(LoginModel loginModel)
        {
            var resultSignIn= await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
            return resultSignIn;
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

        public async Task<IdentityResult> ConfirmEmailAsync(string uid,string token)
        {

            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }


        public async Task<IdentityResult> ResetPasswordAsync(ResetPaswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId),model.Token,model.NewPassword);
        }
    }
}
