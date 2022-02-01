using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public interface IAccountRepository
    {
        Task<int> AddNewUserAsync(LoginModel model);
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> PasswordSignInAsync(LoginModel loginModel);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
    }
}