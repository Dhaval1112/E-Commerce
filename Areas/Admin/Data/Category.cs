using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CetegoryDescription { get; set; }
        public string CoverImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }


    }
}
