using E_Commerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public interface IAddressRepository
    {
        Task<bool> AddNewAddress(AddressModel address);
        List<AddressModel> GetAllAddresses();
        Task<AddressModel> GetSingleAddress(int id);
        Task<bool> EditAddress(AddressModel address);
        Task<bool> DeleteAddress(int id);
    }
}