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

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await _httpClient.GetAsync($"/api/menuitem/{id}");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return NotFound();
        //    }

        //    var jsonData = await response.Content.ReadAsStringAsync();
        //    var menuItem = JsonConvert.DeserializeObject<MenuItemViewModel>(jsonData);

        //    return View("~/Views/Admin/MenuItem/Edit.cshtml", menuItem);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(MenuItemViewModel menuItem)
        //{
        //    var json = JsonConvert.SerializeObject(menuItem);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PutAsync($"/api/menuitem/{menuItem.MenuId}", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return View("Admin/MenuItem/Edit", menuItem);
        //    }

        //    return RedirectToAction("AdminMenu");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _httpClient.DeleteAsync($"/api/menuitem/{id}");

        //    return RedirectToAction("MenuItem");
        //}

        //public IActionResult Add()
        //{
        //    return View("~/Views/Admin/MenuItem/Add.cshtml");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(MenuItemViewModel menuItem)
        //{
        //    var json = JsonConvert.SerializeObject(menuItem);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync("/api/menuitem", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return View("Admin/MenuItem/Add", menuItem);
        //    }

        //    return RedirectToAction("AdminMenu");
        //}

        public async Task<IActionResult> AdminMenu()
        {
            var response = await _httpClient.GetAsync("/api/menuitem");
            var jsonData = await response.Content.ReadAsStringAsync();
            var menuItems = JsonConvert.DeserializeObject<IEnumerable<MenuItemViewModel>>(jsonData);

            return View(menuItems);
        }
    }
}
