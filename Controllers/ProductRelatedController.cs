using E_Commerce.Areas.Admin.Repository;
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

        public ProductRelatedController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
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
    }
}
