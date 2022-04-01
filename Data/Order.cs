using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class Order
    {


        // -----------------User------------------
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }


        // ----------------Address---------------
        public string AddressName { get; set; }
        public string AddressMobile { get; set; }
        public string AddressArea { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressLandmark { get; set; }


        // ----------------Product---------------
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSalerName { get; set; }
        public string ProductCoverImage { get; set; }
        public int ProcuctPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductTotalDiscount { get; set; }

        // -------------Order-------------------
        public int OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
