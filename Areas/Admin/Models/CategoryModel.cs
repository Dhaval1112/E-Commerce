using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name ="Category name"), Required]
        public string CategoryName { get; set; }

        [Display(Name ="Category description"),Required]
        public string CetegoryDescription { get; set; }



        
        [Display(Name ="Choose category image ")]
        public IFormFile CoverImage { get; set; }

        [Display(Name = "Category Url")]
        public string CoverImageUrl { get; set; }



    }
}
