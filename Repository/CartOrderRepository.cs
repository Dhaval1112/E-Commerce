using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using E_Commerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class CartOrderRepository : ICartOrderRepository
    {
        private readonly IUserService _userService;
        private readonly ECommerceContext _context;

        public CartOrderRepository(IUserService userService, ECommerceContext context)
        {
            this._userService = userService;
            this._context = context;
        }
        public async Task<bool> AddToCart(ProductModel product)
        {
            try
            {
                Cart cart = new Cart()
                {
                    Date = DateTime.Now,
                    ProductId = product.Id,
                    Quantity = product.Quantity,
                    UserId = _userService.GetUserId(),
                    

                };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }
            return true;


        }
    }
}
