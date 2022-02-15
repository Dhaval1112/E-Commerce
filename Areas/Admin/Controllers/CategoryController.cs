using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            this._categoryRepository = categoryRepository;
            this._webHostEnvironment = webHostEnvironment;
        }
        

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid) 
            {
                var folder = "Category/Cover/";
                folder += (Guid.NewGuid().ToString() +"_"+ model.CoverImage.FileName); 
                var serverFolder = Path.Combine(_webHostEnvironment.WebRootPath,folder);

                await model.CoverImage.CopyToAsync(new FileStream(serverFolder,FileMode.Create) );

                model.CoverImageUrl = "/"+folder;
                
                var result = await _categoryRepository.CreateCategoryAsync(model);



                if (result > 0)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                }

            }
            return View();
        }
    }
}
