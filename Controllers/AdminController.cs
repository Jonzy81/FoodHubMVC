using FoodHubMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    }
}
