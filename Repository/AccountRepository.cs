using E_Commerce.Data;
using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ECommerceContext _eCommerceContext;

        public AccountRepository(ECommerceContext eCommerceContext)
        {
            this._eCommerceContext = eCommerceContext;
        }
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
    }
}
