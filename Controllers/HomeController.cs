using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Models;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger,IUserService userService,IEmailService emailService, IProductRepository productRepository)
        {
            _logger = logger;
            this._userService = userService;
            this._emailService = emailService;
            this._productRepository = productRepository;
        }

        
        public IActionResult Index()
        {
            // was last Working commit ::  Added profile page with edit functionality 
            //var userId = _userService.GetUserId();
            var AllProducts = _productRepository.GetAllProducts();
            return View(AllProducts);
        }

    
        /*[Authorize(Roles ="Admin,User")]
         
            For multiple roles
         */

        [Authorize(Roles ="User")]
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
