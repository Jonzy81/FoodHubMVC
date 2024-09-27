using Microsoft.AspNetCore.Mvc;

namespace FoodHubMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        public IActionResult AdminLogin()
        {
            ViewBag.NavbarClass = "navbar-dark bg-secondary navbar-static";
            return View();
        }
    }
}
