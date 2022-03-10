using E_Commerce.Areas.Admin.Data;
using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceContext _context;
        private readonly IUserService _userService;

        public ProductRepository(ECommerceContext context,IUserService userService)
        {
            this._context = context;
            this._userService = userService;
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
                Discount = product.Discount,
                SalerName = product.SalerName,
                
          

                
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
                var product = _context.Products.Where(p => p.Id == id).Select(product => new ProductModel()
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
                    CategoryName = product.Category.CategoryName,
                    Gallery = product.productGallery.Select(g => new GalleryModel()
                    {
                        Name = g.Name,
                        Id = g.Id,
                        Url = g.Url
                    }
                    ).ToList(),
                    
                    
                }
                ).FirstOrDefault(); //.FindAsync(id);

                var userId = _userService.GetUserId();
                // for checking that product is already into cart or not..!
                var result=_context.Carts.Where(prod => prod.ProductId == id && prod.UserId == userId).FirstOrDefault();
                product.IsInCart = result != null;
                return product;
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

                if (product.Gallery != null)
                {

                    productData.productGallery = new List<ProductGallery>();
                    foreach (var gallary in product.Gallery)
                    {
                        productData.productGallery.Add(new ProductGallery()
                        {
                            Name = gallary.Name,
                            Url = gallary.Url
                        });
                    }

                }

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

            if (productModel.Gallery!= null)
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


        public bool RemoveGalleryImages(int id,string sysPath)
        {
            var images = _context.productGalleries.Where(glr => glr.ProductId == id);
            foreach (var image in images)
            {
                
                var path = sysPath+image.Url;
                System.IO.File.Delete(path);
            }
            _context.productGalleries.RemoveRange(_context.productGalleries.Where(glr => glr.ProductId == id));
            int result=_context.SaveChanges();
            return result>0;
        }

        public List<CartModel> AllCartProducts()
        {
            var userId=_userService.GetUserId();
            

            List<CartModel> carts = new List<CartModel>();
            var listOfCategory = _context.Users.Where(usr => usr.Id == userId)
                                        .Select(user => user.Carts.Select(
                                            cart => new CartModel
                                            {
                                                Id = cart.Id,
                                                ProductId = cart.ProductId,
                                                ProductName =cart.Product.ProductName,
                                                Quantity = cart.Quantity,
                                                ProductDescription= cart.Product.Description,
                                                Date= cart.Date,
                                                Price= cart.Product.Price,
                                                ProductCategoty= cart.Product.Category.CategoryName,
                                                ProductSaler= cart.Product.SalerName,
                                                ProductCoverImageUrl= cart.Product.CoverImageUrl,
                                                ProductDiscount=cart.Product.Discount
                                                
                                            }
                                            ).ToList()).FirstOrDefault();

            return listOfCategory;



        }
    }
}
