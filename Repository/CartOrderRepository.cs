using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Data;
using E_Commerce.Enums;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.EntityFrameworkCore;
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

        private readonly IAddressRepository addressRepository;
        private readonly IProductRepository productRepository;

        
        public CartOrderRepository(IUserService userService, ECommerceContext context, IAddressRepository addressRepository, IProductRepository productRepository)
        {
            this._userService = userService;
            this._context = context;
            this.addressRepository = addressRepository;
            this.productRepository = productRepository;
        }

        public async Task<bool> RemoveCartProduct(int cartId)
        {
            try
            {
                _context.Carts.Remove(await _context.Carts.FindAsync(cartId));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }

        
        public async Task<bool> AddToCart(ProductModel product,string productStatus)
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

                if (productStatus == "order")
                {
                    cart.CartStatus = 2;
                   
                }
                else
                {
                    cart.CartStatus = 1;

                }
                var cartPoduct=await _context.Carts.AddAsync(cart);
                

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }
            return true;


        }

        public PlaceOrderModel GetPlaceOrderModel()
        {
            PlaceOrderModel placeOrder = new PlaceOrderModel()
            {
                Addresses = addressRepository.GetAllAddresses(),
                Products = productRepository.AllCartProducts("cart"),
            };
            return placeOrder;
        } 



        public PlaceOrderModel CompleteOrder(PlaceOrderModel placeOrderModel)
        {
            /*_context.Database.SqlQuery<>*/
            var userId = _userService.GetUserId();
            
            var orders=_context.Orders.FromSqlRaw<Order>($"PlaceOrder @UserId ='{userId}',@AddressId={placeOrderModel.AddressId}, @CartId =0 ").ToList();


            
            return placeOrderModel;

        }


        public List<Order> GetAllOrders()
        {
            var userId=_userService.GetUserId();

            // Old logic
            // var orders =  _context.Orders.Where(ord=>ord.UserId==userId && (ord.OrderStatus>=1 && ord.OrderStatus<=4)).Select(order => new Order
            
            // new Logic
            var orders =  _context.Orders.Where(ord=>ord.UserId==userId).Select(order => new Order
            {
                Id=order.Id,
                ProductCoverImage = order.ProductCoverImage,
                ProductDescription = order.ProductDescription,
                ProductName = order.ProductName,
                ProductQuantity = order.ProductQuantity,
                OrderStatus = order.OrderStatus,
                ProductSalerName=order.ProductSalerName,
                ProcuctPrice = order.ProcuctPrice

            }).ToList();

            return orders;
        }
    }
}
