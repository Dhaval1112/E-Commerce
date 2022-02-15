using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {


        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _productRepository;

        public ProductController(IWebHostEnvironment webHostEnvironment, IProductRepository productRepository)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._productRepository = productRepository;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                productModel.CoverImageUrl=await UploadImage("Product/Cover/", productModel.CoverImage);
                var id=await _productRepository.AddProductAsync(productModel);
                if (id > 0)
                {
                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                }
            } 
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }




        public async Task<string> UploadImage(string folder, IFormFile file)
        {

           
            folder += (Guid.NewGuid().ToString() + "_" + file.FileName);
            var serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folder;
        }
    }
}
