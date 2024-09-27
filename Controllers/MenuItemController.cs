using Microsoft.AspNetCore.Mvc;

namespace FoodHubMVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(ILogger<MenuItemController> logger)
        {
            _logger = logger;
        }
        public IActionResult MenuItem()
        {
            ViewBag.NavbarClass = "navbar-dark bg-secondary navbar-static";
            return View();
        }
    }
}
