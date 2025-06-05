using coffeeshop.Models;
using coffeeshop.Models.Interfaces;
using coffeeshop.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace coffeeshop.Controllers;


public class HomeController : Controller
{
    private IProductRepository productRepository;

    public HomeController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public IActionResult Index()
    {
        return View(productRepository.GetTrendingProducts());
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
