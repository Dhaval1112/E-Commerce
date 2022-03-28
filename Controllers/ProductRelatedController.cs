using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
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

            var insertedCartId = await _cartOrderRepository.AddToCart(product, productStatus);
            if (insertedCartId>0)
            {
                TempData["IsAddedIntoCart"] = "True";
                TempData["CartCount"] = _cartOrderRepository.getCartCount().ToString();
                if (productStatus == "order")
                {
                    return RedirectToAction("PlaceOrder",new { orderId =insertedCartId});
                }
            }
            else
            {
                TempData["IsAddedIntoCart"] = "False";
            }
            return RedirectToAction("GetProduct",new { id=product.Id});
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult AllCartProducts()
        {
            var carts=_productRepository.AllCartProducts("cart",0);
            return View(carts);
        }

        public async Task<IActionResult> RemoveFromCart(int cartId, string status)
        {
            var isRemoved = await _cartOrderRepository.RemoveCartProduct(cartId);
            TempData["CartCount"] = _cartOrderRepository.getCartCount().ToString();
            TempData["IsRemoved"] = "True";


            // for checking that it's removed from cart or order page
            return RedirectToAction(status == null ? "AllCartProducts": "pandingorders");
            
        }

        //this view only for select options like address,payment method, etc
        public IActionResult PlaceOrder(int? orderId)
        {
            if(orderId == null)
            {
                // when we do want to checkout from cart page
                var order =_cartOrderRepository.GetPlaceOrderModel(0);
               
                return View(order);

            }
            else
            {
                var order = _cartOrderRepository.GetPlaceOrderModel((int)orderId);
                return View(order);
            }          
        }


        //this is for placing order 
        [HttpPost]
        public IActionResult PlaceOrder(PlaceOrderModel placeOrderModel)
        {
            var order = _cartOrderRepository.GetPlaceOrderModel(placeOrderModel.CartProductId);
            if (_cartOrderRepository.CompleteOrder(placeOrderModel))
            {
                TempData["IsOrder"] = "True";

            }
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
            var pandingOrders= _productRepository.AllCartProducts("order",0);
            return View(pandingOrders);
        }

        public async Task<IActionResult> CancelOrder(int id)
        {
            if (await _cartOrderRepository.CancelOrder(id))
            {
                TempData["IsOrderCancel"] = "True"; 
            }
            else
            {
                TempData["IsOrderCancel"] = "False"; 

            }
            return RedirectToAction("AllOrders");
        }
    }
}
