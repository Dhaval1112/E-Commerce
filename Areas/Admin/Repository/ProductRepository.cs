using E_Commerce.Areas.Admin.Data;
using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using E_Commerce.Enums;
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



        // TODO: this is for future enhancement maping and taking status of order
        private void setStatus(Cart crt,ProductModel prdModel)
        {
            if (!prdModel.IsInCart)
                prdModel.IsInCart = crt.CartStatus == (int)AddTo.Cart;

            
            if(!prdModel.IsInOrder)
                prdModel.IsInOrder= crt.CartStatus == (int)AddTo.Order;
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
                //var isInCartResult = _context.Carts.Where(prod => prod.ProductId == id && prod.UserId == userId).ToList() ;

                var isInCartResult = _context.Carts.Where(prod => prod.ProductId == id && prod.UserId == userId && prod.CartStatus == (int)AddTo.Cart).FirstOrDefault();

                var isInOrderResult=_context.Carts.Where(prod => prod.ProductId == id && prod.UserId == userId && prod.CartStatus == (int)AddTo.Order).FirstOrDefault();

                if (isInCartResult != null)
                {
                    product.IsInCart = isInCartResult.CartStatus == (int) AddTo.Cart;

                }
                if(isInOrderResult !=null)
                {

                    product.IsInOrder = isInOrderResult.CartStatus == (int)AddTo.Order;
                    product.cartId = isInOrderResult.Id;
                }

                return product;
            }
            catch (Exception e)
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

        private static int CalculateDiscount(float price,float discount,int quantity)
        {
            return (int) Math.Ceiling(((price* discount)/100)*quantity);
        }

        

        public List<CartModel> AllCartProducts(string productsStatus,int cartId=0)
        {
            var userId=_userService.GetUserId();

            #region Writing_where_type_query
            /*
            var listOfCartDEMO = (from usr in _context.Users
                                  join crt in _context.Carts on usr.Id equals crt.UserId
                                  join prd in _context.Products on crt.ProductId equals prd.Id
                                  where (usr.Id == userId && crt.CartStatus==(int) AddTo.Order) 
                                  select  new CartModel
                                  {
                                      Id = crt.Id,
                                      ProductId = crt.ProductId,
                                      ProductName = crt.Product.ProductName,
                                      Quantity = crt.Quantity,
                                      ProductDescription = crt.Product.Description,
                                      Date = crt.Date,
                                      Price = crt.Product.Price,
                                      ProductCategoty = crt.Product.Category.CategoryName,
                                      ProductSaler = crt.Product.SalerName,
                                      ProductCoverImageUrl = crt.Product.CoverImageUrl,
                                      ProductDiscount = crt.Product.Discount,
                                      TotalPrice = Convert.ToInt32(crt.Product.Price * crt.Quantity),
                                      DiscountedPrice = (crt.Product.Price * crt.Quantity) - CalculateDiscount(crt.Product.Price, crt.Product.Discount, crt.Quantity),
                                      TotalDiscount = CalculateDiscount(crt.Product.Price, crt.Product.Discount, crt.Quantity),
                                      //Convert.ToInt32((cart.Product.Price * cart.Quantity)-((cart.Product.Price * cart.Quantity) * cart.Product.Discount == 0 ? 1 : cart.Product.Discount) / 100)
                                  }).ToList();


            */

            #endregion 


            if (cartId==0)
            { 


                              
                var listOfCarts= _context.Users.Where(usr => usr.Id == userId)
                                            .Select(user => user.Carts.Where(crt=> crt.CartStatus== (productsStatus == "order"? (int) AddTo.Order : (int)AddTo.Cart ) ).Select(
                                            
                                                cart => new CartModel
                                                {
                                                    Id = cart.Id,
                                                    ProductId = cart.ProductId,
                                                    ProductName = cart.Product.ProductName,
                                                    Quantity = cart.Quantity,
                                                    ProductDescription = cart.Product.Description,
                                                    Date = cart.Date,
                                                    Price = cart.Product.Price,
                                                    ProductCategoty = cart.Product.Category.CategoryName,
                                                    ProductSaler = cart.Product.SalerName,
                                                    ProductCoverImageUrl = cart.Product.CoverImageUrl,
                                                    ProductStock = cart.Product.Stock,
                                                    ProductDiscount = cart.Product.Discount,
                                                    TotalPrice = Convert.ToInt32(cart.Product.Price * cart.Quantity),
                                                    DiscountedPrice = (cart.Product.Price * cart.Quantity) - CalculateDiscount(cart.Product.Price, cart.Product.Discount, cart.Quantity),
                                                    TotalDiscount= CalculateDiscount(cart.Product.Price,cart.Product.Discount,cart.Quantity),
                                                    //Convert.ToInt32((cart.Product.Price * cart.Quantity)-((cart.Product.Price * cart.Quantity) * cart.Product.Discount == 0 ? 1 : cart.Product.Discount) / 100)
                                                }
                                                ).ToList()).FirstOrDefault();

                 return listOfCarts;
            }
            else
            {
                var listOfCarts = _context.Users.Where(usr => usr.Id == userId)
                                            .Select(user => user.Carts.Where(crt => ( crt.CartStatus == (productsStatus == "order" ? (int)AddTo.Order : (int)AddTo.Cart) ) && crt.Id==cartId).Select(

                                                cart => new CartModel
                                                {
                                                    Id = cart.Id,
                                                    ProductId = cart.ProductId,
                                                    ProductName = cart.Product.ProductName,
                                                    ProductStock=cart.Product.Stock,
                                                    Quantity = cart.Quantity,
                                                    ProductDescription = cart.Product.Description,
                                                    Date = cart.Date,
                                                    Price = cart.Product.Price,
                                                    ProductCategoty = cart.Product.Category.CategoryName,
                                                    ProductSaler = cart.Product.SalerName,
                                                    ProductCoverImageUrl = cart.Product.CoverImageUrl,
                                                    ProductDiscount = cart.Product.Discount,
                                                    TotalPrice = Convert.ToInt32(cart.Product.Price * cart.Quantity),
                                                    DiscountedPrice = (cart.Product.Price * cart.Quantity) - CalculateDiscount(cart.Product.Price, cart.Product.Discount, cart.Quantity),
                                                    TotalDiscount = CalculateDiscount(cart.Product.Price, cart.Product.Discount, cart.Quantity),
                                                    //Convert.ToInt32((cart.Product.Price * cart.Quantity)-((cart.Product.Price * cart.Quantity) * cart.Product.Discount == 0 ? 1 : cart.Product.Discount) / 100)
                                                }
                                                ).ToList()).FirstOrDefault();

                return listOfCarts;
            }



        }



        public List<ProductModel> SearchProduct(string productName)
        {
            var products=_context.Products.Where(product => product.ProductName.Contains(productName) || product.Category.CategoryName.Contains( productName)).Select( product=>
                new ProductModel()
                {

                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description=product.Description,
                    CoverImageUrl = product.CoverImageUrl,
                    Price = product.Price,
                    Stock = product.Stock,
                    Discount = product.Discount,
                    SalerName = product.SalerName,

                }).ToList();
            return products;
            
        }
        public List<ProductModel> SearchProductByCategoryName(string categoryname)
        {

            var products = _context.Products.Where(product => product.Category.CategoryName == categoryname ).Select(product =>
                new ProductModel()
                {

                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    CoverImageUrl = product.CoverImageUrl,
                    Price = product.Price,
                    Stock = product.Stock,
                    Discount = product.Discount,
                    SalerName = product.SalerName,

                }).ToList();
            return products;


        }

    }
}
