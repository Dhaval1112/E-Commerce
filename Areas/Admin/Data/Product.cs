using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public int Discount { get; set; }
        public string SalerName { get; set; }
        public string CoverImageUrl { get; set; }

        public Category Category { get; set; }
    }
}
