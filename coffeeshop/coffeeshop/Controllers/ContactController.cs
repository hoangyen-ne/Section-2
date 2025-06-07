using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
