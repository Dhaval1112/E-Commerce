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

        public HomeController(ILogger<HomeController> logger,IUserService userService,IEmailService emailService)
        {
            _logger = logger;
            this._userService = userService;
            this._emailService = emailService;
        }

        
        public IActionResult Index()
        {
            /*UserEmailOptions userEmailOptions = new UserEmailOptions()
            {
                Subject = "Testing email",
                ToEmail = "dhavalvaghela1112@gmail.com",
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}","Dhaval")
                }
            };


            await _emailService.sendTestEmail(userEmailOptions);*/


            var userId = _userService.GetUserId();
            return View();
        }

    
        /*[Authorize(Roles ="Admin,User")]
         
            For multiple roles
         */

        [Authorize(Roles ="User")]
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
