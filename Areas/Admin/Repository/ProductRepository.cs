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


        public  List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products =_context.Products.Select(product => new ProductModel()
            {
                Id = product.Id,
                ProductName= product.ProductName,
                CoverImageUrl = product.CoverImageUrl,
                Price = product.Price,
                Stock = product.Stock,
                SalerName = product.SalerName
                
            }).ToList();


            return products;
        } 

        public async Task<bool> DeleteProductAsync(int id)
        {
           
            try
            {

                var product = await getProductData(id);
                var result =_context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return result != null;
            }catch(Exception)
            {

                return false;                
            }
        }

        // get actual product details
        private async Task<Product> getProductData(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // for getting product model
        public ProductModel GetProductModel(int id)
        {

            try
            {
                return  _context.Products.Where(p => p.Id == id).Select(product => new ProductModel()
                {
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    CoverImageUrl = product.CoverImageUrl,
                    Description = product.Description,
                    Discount = product.Discount,
                    SalerName = product.SalerName,
                    Price = product.Price,
                    Stock = product.Stock,
                    Id = product.Id,
                    Gallery = product.productGallery.Select(g => new GalleryModel()
                    {
                        Name = g.Name,
                        Url = g.Url
                    }
                    ).ToList(),
                    
                }
                ).FirstOrDefault(); //.FindAsync(id);


            }
            catch (Exception)
            {
                return null;             
            }
        }

        
        public async Task<bool> EditProduct(ProductModel product)
        {


            try
            {
                Product productData = await getProductData(product.Id);

                productData.ProductName = product.ProductName;
                productData.CategoryId = product.CategoryId;
                productData.Discount = product.Discount;
                productData.Description = product.Description;
                productData.SalerName = product.SalerName;
                productData.Stock = product.Stock;
                productData.Price = product.Price;
                productData.CoverImageUrl = product.CoverImageUrl;


                _context.Products.Update(productData);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            


            return true;
        }

        public async Task<int> AddProductAsync(ProductModel productModel)
        {
            Product product = new Product()
            {
                CategoryId = productModel.CategoryId,
                CoverImageUrl = productModel?.CoverImageUrl,
                Discount = productModel.Discount,
                Description = productModel.Description,
                Price = productModel.Price,
                SalerName = productModel.SalerName,
                ProductName = productModel.ProductName,
                Stock = productModel.Stock
            };

            if (product?.productGallery!= null)
            {

                product.productGallery = new List<ProductGallery>();
                foreach (var gallary in productModel.Gallery)
                {
                    product.productGallery.Add(new ProductGallery()
                    {
                        Name = gallary.Name,
                        Url = gallary.Url
                    }) ;
                }
                
            }
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();


        }
    }
}
