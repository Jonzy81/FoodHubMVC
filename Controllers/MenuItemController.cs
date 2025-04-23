using FoodHubMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Text;

namespace FoodHubMVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ILogger<MenuItemController> _logger;
        private readonly HttpClient _httpClient;

        public MenuItemController(ILogger<MenuItemController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7279");
        }
        public async Task<IActionResult> MenuItem()
        {
            ViewBag.NavbarClass = "navbar-dark bg-secondary navbar-static";

            var response = await _httpClient.GetAsync("/api/menuitem");
            var jsonData = await response.Content.ReadAsStringAsync();

            var menuItemsViewModel = JsonConvert.DeserializeObject<IEnumerable<MenuItemViewModel>>(jsonData);
            return View(menuItemsViewModel);
        }

        public async Task<IActionResult> AdminMenu()
        {
            var response = await _httpClient.GetAsync("/api/menuitem");
            var jsonData = await response.Content.ReadAsStringAsync();
            var menuItems = JsonConvert.DeserializeObject<IEnumerable<MenuItemViewModel>>(jsonData);

            return View(menuItems);
        }
    }
}
