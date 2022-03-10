using E_Commerce.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public Product Product{ get; set; }
        public int ProductId{ get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date{ get; set; }
    }
}
