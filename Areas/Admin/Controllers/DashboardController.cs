using E_Commerce.Areas.Admin.Repository;
using E_Commerce.Data;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly ICartOrderRepository _cartOrderRepository;
        private readonly IOrderRepository _orderRepository;

        public DashboardController(ICartOrderRepository cartOrderRepository, IOrderRepository orderRepository)
        {
            this._cartOrderRepository = cartOrderRepository;
            this._orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            var dashboardModel=_cartOrderRepository.GetAllUserOrders();
            return View(dashboardModel);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            // https://localhost:44381/admin/dashboard/changestatus/347
            var order =await _orderRepository.GetOrder(id);

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Order order)
        {
            var oldOrder=await _orderRepository.UpdateOrderStatus(order);   
            if(oldOrder != null)
            {
                TempData["IsUpdateOrder"] = "true";
            }
            return View(oldOrder);            
        }
    }
}
