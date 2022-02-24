using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required, Display(Name ="Product name ")]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, Display(Name ="Select product's category ") ]
        public int CategoryId { get; set; }

        public float Price { get; set; }
        public int Stock { get; set; }
        [Range(0,100),Display(Name ="Discount % ")]


        public int Discount { get; set; }

        [Required, Display(Name ="Saler name ")]
        public string SalerName { get; set; }

        public IFormFile CoverImage { get; set; }
        public String  CoverImageUrl { get; set; }
        public IFormFileCollection GalleryFiles { get; set; }
        public List<GalleryModel> Gallery { get; set; }


    }
}
