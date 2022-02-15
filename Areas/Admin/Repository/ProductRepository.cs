using E_Commerce.Areas.Admin.Data;
using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceContext _context;

        public ProductRepository(ECommerceContext context)
        {
            this._context = context;
        }

        public async Task<int> AddProductAsync(ProductModel productModel)
        {
            Product product = new Product()
            {
                CategoryId = productModel.CategoryId,
                CoverImageUrl = productModel.CoverImageUrl,
                Discount = productModel.Discount,
                Description = productModel.Description,
                Price = productModel.Price,
                SalerName = productModel.SalerName,
                ProductName = productModel.ProductName,
                Stock = productModel.Stock
            };

            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();


        }
    }
}
