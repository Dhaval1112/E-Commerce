using E_Commerce.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class PlaceOrderModel
    {
        public List<AddressModel> Addresses;
        public List<CartModel> Products;
        
        [Required(ErrorMessage ="Selecting address is required..!")]
        public int AddressId { get; set; }
        [Required(ErrorMessage ="Select appropriate payment option..!")]
        public int PaymentMethodId { get; set; }

    }
}
