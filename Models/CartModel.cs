using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategoty { get; set; }
        public string ProductSaler { get; set; }
        public string ProductCoverImageUrl { get; set; }
        public float Price { get; set; }
        public int ProductDiscount { get; set; }
        public DateTime Date { get; set; }

        public float DiscountedPrice { get; set; }
        public float TotalPrice { get; set; }
        public float TotalDiscount { get; set; }
        public int FromWhichAction { get; set; }
        public int ProductStock { get; set; }


    }
}
