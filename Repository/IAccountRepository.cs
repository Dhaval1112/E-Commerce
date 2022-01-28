using E_Commerce.Models;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public interface IAccountRepository
    {
        Task<int> AddNewUserAsync(LoginModel model);
    }
}