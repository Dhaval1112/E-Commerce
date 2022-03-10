using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
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

        public IActionResult AddToCart(ProductModel product)
        {
            _cartOrderRepository.AddToCart(product);
            return RedirectToAction("GetProduct",new { id=product.Id});
        }

        public IActionResult AllCartProducts()
        {
            var carts=_productRepository.AllCartProducts();
            return View(carts);
        }
    }
}
