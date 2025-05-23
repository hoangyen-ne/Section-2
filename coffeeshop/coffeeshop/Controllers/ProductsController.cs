using coffeeshop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace coffeeshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Shop()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }
    }
}
