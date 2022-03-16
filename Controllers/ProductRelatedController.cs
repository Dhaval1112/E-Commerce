using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace E_Commerce.Controllers
{
    public class ProductRelatedController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartOrderRepository _cartOrderRepository;

        public ProductRelatedController(IProductRepository productRepository, ICartOrderRepository cartOrderRepository)
        {
            this._productRepository = productRepository;
            this._cartOrderRepository = cartOrderRepository;
        }
        public IActionResult GetProduct(int id)
        {

            var product = _productRepository.GetProductModel(id);
            return View(product);
        }

        public IActionResult GetProductDetails(int id)
        {
            var product = _productRepository.GetProductModel(id);
            return View(product);
        }

        public async Task<IActionResult> AddToCart(ProductModel product, string productStatus)
        {

            
            if (await _cartOrderRepository.AddToCart(product,productStatus))
            {
                TempData["IsAddedIntoCart"] = "True";
            }
            else
            {
                TempData["IsAddedIntoCart"] = "False";
            }
            return RedirectToAction("GetProduct",new { id=product.Id});
        }

        public IActionResult AllCartProducts()
        {
            var carts=_productRepository.AllCartProducts("cart");
            return View(carts);
        }

        public async Task<IActionResult> RemoveFromCart(int cartId, string status)
        {
            var isRemoved = await _cartOrderRepository.RemoveCartProduct(cartId);
            TempData["IsRemoved"] = "True";


            // for checking that it's removed from cart or order page
            return RedirectToAction(status == null ? "AllCartProducts": "PlaceOrder");
            
        }


        //this view only for select options like address,payment method, etc
        public IActionResult PlaceOrder(int? id)
        {
            if(id==null)
            {
                // when we do want to checkout from cart page
                var order =_cartOrderRepository.GetPlaceOrderModel();
               
                return View(order);

            }
            else
            {
                var order = _cartOrderRepository.GetPlaceOrderModel();
                return View(order);
            }          
        }


        //this is for placing order 
        [HttpPost]
        public IActionResult PlaceOrder(PlaceOrderModel placeOrderModel)
        {
            var order = _cartOrderRepository.GetPlaceOrderModel();
            var newOrder=_cartOrderRepository.CompleteOrder(placeOrderModel);
            return View(order);
        }


        // this is for getting all ordered products by this user
        public  IActionResult AllOrders()
        {
            var orders=_cartOrderRepository.GetAllOrders();
            return View(orders);
        }


        public IActionResult PlaceSingleOrder(ProductModel product)
        {

            return View();
        }


        public IActionResult PandingOrders()
        {
            var pandingOrders= _productRepository.AllCartProducts("order");
            return View(pandingOrders);
        }
    }
}
