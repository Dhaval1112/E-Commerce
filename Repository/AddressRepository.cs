using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace E_Commerce.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ECommerceContext _context;
        private readonly IUserService _userService;

        public AddressRepository(ECommerceContext context, IUserService userService)
        {
            this._context = context;
            this._userService = userService;
        }




        

        // For adding address from model to database
        public async Task<bool> AddNewAddress(AddressModel address)
        {
            try
            {
                Address newAddress = new Address()
                {
                    Name = address.Name,
                    Pincode = address.Pincode,
                    City = address.City,
                    AddressArea = address.AddressArea,
                    Landmark = address.Landmark,
                    MobileNumber = address.MobileNumber,
                    State = address.State,
                    userId = _userService.GetUserId()
                };
                await _context.Addresses.AddAsync(newAddress);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }



            return true;
        }


        // Get Single Address
        public async Task<AddressModel> GetSingleAddress(int id)
        {
            try
            {

                var address =await _context.Addresses.FindAsync(id);
                var addressModel = new AddressModel()
                {
                    AddressArea = address.AddressArea,
                    City = address.City,
                    Id = address.Id,
                    Landmark = address.Landmark,
                    MobileNumber = address.MobileNumber,
                    Name = address.Name,
                    Pincode = address.Pincode,
                    State = address.State
                };
            return addressModel;
            }
            catch (Exception)
            {

                return null;
            }
        }

        // For getting all addresses of user from database
        public List<AddressModel> GetAllAddresses()
        {
            var addresses = _context.Addresses.Where(add => add.userId == _userService.GetUserId()).Select(
                address => new AddressModel
                {
                    Id = address.Id,
                    AddressArea = address.AddressArea,
                    City = address.City,
                    MobileNumber = address.MobileNumber,
                    Name = address.Name,
                    Landmark = address.Landmark,
                    Pincode = address.Pincode,
                    State = address.State
                }
                ).ToList();
            return addresses;
        }


        // for editing existing address 
        public async Task<bool> EditAddress(AddressModel address)
        {
            var oldAddress=await _context.Addresses.FindAsync(address.Id);
            oldAddress.City = address.City;
            oldAddress.MobileNumber = address.MobileNumber;
            oldAddress.Name = address.Name;
            oldAddress.Pincode = address.Pincode;
            oldAddress.State = address.State;
            oldAddress.Landmark = address.Landmark;

            _context.Update(oldAddress);
            await _context.SaveChangesAsync();
           return true;

        }

        public async Task<bool> DeleteAddress(int id)
        {
            try
            {

                var address = await _context.Addresses.FindAsync(id);
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception)
            {

                return false;
            }
            return true;

        }
    }
}
