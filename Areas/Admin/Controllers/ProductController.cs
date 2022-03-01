using E_Commerce.Areas.Admin.Data;
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
                if(productModel.CoverImage != null)
                {
                    productModel.CoverImageUrl=await UploadImage("Product/Cover/", productModel.CoverImage);
                }

                if (productModel.GalleryFiles != null)
                {
                    string folder = "Product/Gallery/";
                    productModel.Gallery = new List<GalleryModel>();

                    foreach (var file in productModel.GalleryFiles)
                    {
                        var newGallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            Url = await UploadImage(folder, file)
                        };
                        productModel.Gallery.Add(newGallery);
                        
                    }

                }
                var id=await _productRepository.AddProductAsync(productModel);
                if (id > 0)
                {
                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                }
            } 
            return View();
        }


        public  IActionResult EditProduct(int id)
        {
            var product = _productRepository.GetProductModel(id);
            if (product != null)
            {
                return View(product);

            }
            else
            {
                return View(new ProductModel() );
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model)
        {
            if (model.CoverImage != null)
            {
                model.CoverImageUrl = await UploadImage("Product/Cover/", model.CoverImage);
                
            }

            
            if (model.GalleryFiles != null)
            {
                var path = _productRepository.RemoveGalleryImages(model.Id, _webHostEnvironment.WebRootPath);


                //var result = _productRepository.RemoveGalleryImages(model.Id,_webHostEnvironment.WebRootPath);
                
                string folder = "Product/Gallery/";
                model.Gallery = new List<GalleryModel>();

                foreach (var file in model.GalleryFiles)
                {

                    var newGallery = new GalleryModel()
                    {
                        Name = file.FileName,
                        Url = await UploadImage(folder, file)
                    };
                    model.Gallery.Add(newGallery);

                }

            }

            TempData["IsUpdated"]= await _productRepository.EditProduct(model);

            //return View(model); 

            return RedirectToAction("GetAllProducts");
        }

        
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var result = await _productRepository.DeleteProductAsync(Id);
            if (result)
            {
                var successDeleteImages = _productRepository.RemoveGalleryImages(Id, _webHostEnvironment.WebRootPath);
                TempData["IsDeleted"] = "Success";
            }
            else
            {
                TempData["IsDeleted"] = "Fail";

            }
            return RedirectToAction("GetAllProducts");

        }

        public IActionResult GetAllProducts()
        {
            var products = _productRepository.GetAllProducts();

            return View(products);
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
