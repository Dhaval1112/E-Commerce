using E_Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceContext _context;

        public OrderRepository(ECommerceContext context)
        {
            this._context = context;
        }
        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order;
        }

        public async Task<Order> UpdateOrderStatus(Order order)
        {
            var oldOrder = await GetOrder(order.Id);

            oldOrder.OrderStatus = order.OrderStatus;
            _context.Orders.Update(oldOrder);
            await _context.SaveChangesAsync();

            return oldOrder;

        }
    }
}
