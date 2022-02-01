using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /*Login for returning view*/
        public IActionResult Login()
        {
            return View();
        }

        /*For getting data*/
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result=await _accountRepository.PasswordSignInAsync(model);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);

                    }
                    return RedirectToAction("Index","Home");
                }
                /*ModelState.Clear();   */
                ModelState.AddModelError("","Invalid credentials..");
            }

            return View();
        }

        /*For returning view*/
        [Route("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        /*For getting data*/
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {

            //ABCabc@123
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.isSuccess = true;
                }
            }

            
            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [Route("change-password")]
        public IActionResult changePassword()
        {

            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> changePassword(ChangePasswordModel changePassword)
        {


            if (ModelState.IsValid)
            {
                var result =await _accountRepository.ChangePasswordAsync(changePassword);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }


            }
            return View(changePassword);
        }

    }

}
