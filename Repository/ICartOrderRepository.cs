using E_Commerce.Areas.Admin.Models;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public interface ICartOrderRepository
    {
        Task<bool> AddToCart(ProductModel product);
    }
}