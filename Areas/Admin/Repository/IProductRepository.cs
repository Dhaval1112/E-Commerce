using E_Commerce.Areas.Admin.Models;
using E_Commerce.Models;
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
        public List<CartModel> AllCartProducts(string productsStatus, int cartId);
        List<ProductModel> SearchProduct(string productName);
        List<ProductModel> SearchProductByCategoryName(string categoryname);
    }
}


