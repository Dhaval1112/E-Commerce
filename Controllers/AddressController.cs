using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public IActionResult AllAddresses ()
        {
            var addresses = _addressRepository.GetAllAddresses();
            return View(addresses);
        }
        public IActionResult CreateAddress()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressModel address)
        {
            if (ModelState.IsValid)
            {
                if(await _addressRepository.AddNewAddress(address))
                {
                    TempData["IsSuccess"] = "Success";
                    ModelState.Clear();
                    return RedirectToAction("AllAddresses");
                }
                else
                {
                
                    TempData["IsSuccess"] = "Fail";
                }
            }
            return View();
        }

        
        public async Task<IActionResult> EditAddress(int id)
        {
            var address = await _addressRepository.GetSingleAddress(id);
            return View(address);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressModel address)
        {
            var result = await _addressRepository.EditAddress(address);
            if(result)
                TempData["IsSuccess"] = "Success";
            else
                TempData["IsSuccess"] = "Fail";
                   
            return View(address);
        }
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if(await _addressRepository.DeleteAddress(id))
            {
                TempData["IsDeleted"] = "Success";
            }
            else
            {

                TempData["IsDeleted"] = "Fail";
            }
            
            return RedirectToAction("AllAddresses");
        }
    }
}
