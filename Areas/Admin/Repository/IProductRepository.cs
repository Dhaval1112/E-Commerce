using E_Commerce.Areas.Admin.Models;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public interface IProductRepository
    {
        Task<int> AddProductAsync(ProductModel productModel);
    }
}