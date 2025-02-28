using FoodHubMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace FoodHubMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly HttpClient _httpClient;

        public AdminController(ILogger<AdminController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7279");
        }

        [Authorize]
        public async Task<IActionResult> AdminMenu()
        {
            ViewBag.NavbarClass = "navbar-dark bg-secondary navbar-static";

            var response = await _httpClient.GetAsync("/api/menuitem");

            var jsonData = await response.Content.ReadAsStringAsync();
            var menuItemsViewModel = JsonConvert.DeserializeObject<IEnumerable<MenuItemViewModel>>(jsonData);

           

            return View(menuItemsViewModel);
        }
        public IActionResult AdminLogin()
        {
            ViewBag.NavbarClass = "navbar-dark bg-secondary navbar-static";
            ViewBag.Body = "bg-primary";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", loginUser);   //Gör anrop mot api

            if (!response.IsSuccessStatusCode)
            {
                return View(loginUser);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();  //saves claims (name, password etc) 

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwtToken.ValidTo
            });

            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            return RedirectToAction("AdminMenu", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("Add");  
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(MenuItemViewModel menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/menuitem", content);

            if (!response.IsSuccessStatusCode)
            {
                return View("Add", menuItem);
            }

            return RedirectToAction("AdminMenu");
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/menuitem/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItemViewModel>(jsonData);

            return View("Edit", menuItem); // Will look for /Views/Admin/MenuItem/Edit.cshtml
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(MenuItemViewModel menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/menuitem/{menuItem.MenuId}", content);

            if (!response.IsSuccessStatusCode)
            {
                return View("Edit", menuItem);
            }

            return RedirectToAction("AdminMenu");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/menuitem/{id}");

            return RedirectToAction("AdminMenu");
        }
    }
}
