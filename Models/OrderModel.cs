using E_Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class OrderModel
    {

        public List<Order> orders { get; set; }
        public int numberOfProducts { get; set; }
        public int numberOfOrders { get; set; }
        public int Sales { get; set; }

    }
}
