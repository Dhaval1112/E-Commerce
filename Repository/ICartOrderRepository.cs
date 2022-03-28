using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using E_Commerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public interface ICartOrderRepository
    {
        Task<int> AddToCart(ProductModel product, string productStatus);
        Task<bool> RemoveCartProduct(int productId);
        PlaceOrderModel GetPlaceOrderModel(int id);
        bool CompleteOrder(PlaceOrderModel order);
        List<Order> GetAllOrders();
        OrderModel GetAllUserOrders();
        int getCartCount();
        Task<bool> CancelOrder(int id);
    }
}