using FoodHubMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodHubMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Read the content of the text file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "AboutText.txt");
            string aboutText = System.IO.File.ReadAllText(filePath);

            // Replace newlines (\n) with <br> tags
            aboutText = aboutText.Replace("\n", "<br>");

            // Pass the modified text to the view
            ViewData["AboutText"] = aboutText;

            return View();
        }

        public IActionResult Menu()
        {
            ViewBag.NavbarClass = "navbar-dark bg-dark navbar-static";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
