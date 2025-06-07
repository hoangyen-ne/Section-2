using coffeeshop.Models;
using coffeeshop.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Checkout model)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = " Order placed successfully!";
                return RedirectToAction("Checkout");
            }

            return View(model); 
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
