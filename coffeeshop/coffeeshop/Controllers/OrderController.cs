using coffeeshop.Models;
using coffeeshop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace coffeeshop.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderRepository orderRepository;
        private IShoppingCartRepository shoppingCartRepository;
        public OrdersController(IOrderRepository oderRepository,
       IShoppingCartRepository shoppingCartRepossitory)
        {
            this.orderRepository = oderRepository;
            this.shoppingCartRepository = shoppingCartRepossitory;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            orderRepository.PlaceOrder(order);
            shoppingCartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", order.Id);  
            return RedirectToAction("CheckoutComplete");
        }
        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
