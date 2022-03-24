using E_Commerce.Data;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrder(int id);
        Task<Order> UpdateOrderStatus(Order order);
    }
}