using E_Commerce.Areas.Admin.Models;
using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartOrderRepository _cartOrderRepository;

        public HomeController(ILogger<HomeController> logger,IUserService userService,IEmailService emailService, IProductRepository productRepository, ICategoryRepository categoryRepository,ICartOrderRepository cartOrderRepository)
        {
            _logger = logger;
            this._userService = userService;
            this._emailService = emailService;
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
            this._cartOrderRepository = cartOrderRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            UserHomePageModel userHomePageModel = new UserHomePageModel();
            userHomePageModel.Products = _productRepository.GetAllProducts();
            userHomePageModel.Categories =_categoryRepository.AllCategories().GetRange(1,4);

            TempData["CartCount"] = _cartOrderRepository.getCartCount().ToString();
            return View(userHomePageModel);
        }
        

        [AllowAnonymous]
        public IActionResult SearchProductByName(string productName,string categoryName)
        {
            if (productName != null)
            {

                return View( _productRepository.SearchProduct(productName));
            }
            else
            {
                return View(_productRepository.SearchProductByCategoryName(categoryName));
            }
        }

    
        /*[Authorize(Roles ="Admin,User")]
         
            For multiple roles
         */

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        /*public string Message(string name)
        {
            return "hello "+name;
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        
    }
}
