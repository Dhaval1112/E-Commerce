using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICartOrderRepository _cartOrderRepository;
        private readonly IUserService _userService;

        public AccountController(IAccountRepository accountRepository, ICartOrderRepository cartOrderRepository,IUserService userService)
        {
            _accountRepository = accountRepository;
            this._cartOrderRepository = cartOrderRepository;
            this._userService = userService;
        }


        // For getting profile details
        public async Task<IActionResult> Profile(){

            var user = await _accountRepository.GetLogedInUser();
            return View(user);

        }
        
        // For Updating profile details
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileModel user){

            if (ModelState.IsValid)
            {
                var result =await _accountRepository.UpdateProfile(user);
                if (result.Succeeded)
                {
                    user.IsSuccess = true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }

            }
            return View(user);

        }

        /*Login for returning view*/
        public IActionResult Login()
        {
            if (_userService.IsAuthanticated())
            {

              
                return RedirectToAction("Index","Home",new { refresh="a"});
            }
            else
            {
                return View();
            }
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
                    var roles = await _accountRepository.GetRoleByEmail(model.Email);
                    

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (roles.Contains("User"))
                        {

                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToRoute(new { action = "Index", controller = "Dashboard", area = "admin" });
                        }

                    }
                    return RedirectToAction("Index","Home");
                }

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("","Not allowed to login please confirm your email..");

                }
                else
                {

                    /*ModelState.Clear();   */
                    ModelState.AddModelError("","Invalid credentials..");
                }
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
                    return View(userModel);
                }
                else
                {
                    ModelState.Clear();
                    return RedirectToAction("ConfirmEmail", new { email = userModel.Email });                    
                }   
            }

            
            return View();
        }
             
        [Route("logout")]
        [Authorize(Roles ="Admin,User")]
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


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email,
            };

            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result=await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVarified = true;
                }
            }
            return View(model);
        }

        
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel emailConfirmModel)
        {
            var user = await _accountRepository.GetUSerByEmailAsync(emailConfirmModel.Email);
            if(user != null)
            {
                if (user.EmailConfirmed)
                {
                    emailConfirmModel.IsConfirmed = true;
                    return View(emailConfirmModel); 
                }
                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                emailConfirmModel.EmailSent = true;
                ModelState.Clear();

            }
            else
            {
                ModelState.AddModelError("", "Something went wrong..!");
            }
            return View(emailConfirmModel);
        }


        [AllowAnonymous,HttpGet("forgot-password"),]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _accountRepository.GetUSerByEmailAsync(model.Email);
                if(user != null)
                {
                    await _accountRepository.GeneratePasswordResetTokenAsync(user);


                }

                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password"),]
        public IActionResult ResetPassword(string uid,string token)
        {
            ResetPaswordModel resetPaswordModel = new ResetPaswordModel
            {
                UserId = uid,
                Token = token
            };
            return View(resetPaswordModel);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword(ResetPaswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token=model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
                

            }
            return View(model);
        }

    }

}
