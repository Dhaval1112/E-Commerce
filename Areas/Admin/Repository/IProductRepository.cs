using E_Commerce.Areas.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public interface IProductRepository
    {
        Task<int> AddProductAsync(ProductModel productModel);
        List<ProductModel> GetAllProducts();
        Task<bool> DeleteProductAsync(int id);
        ProductModel GetProductModel(int id);

        Task<bool> EditProduct(ProductModel product);

        bool RemoveGalleryImages(int id, string sysPath);
    }
}