using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace mvc_client.Controllers
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
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Request.Cookies.TryGetValue("IdToken", out var idToken);

            // 1. удаляем все куки (это умеет делать сам envoy, но тут проще)
            Response.Cookies.Append("IdToken", string.Empty, new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(-1)
            });
            Response.Cookies.Append("AccessToken", string.Empty, new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(-1)
            });
            Response.Cookies.Append("RefreshToken", string.Empty, new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(-1) 
            });
            
            Response.Cookies.Append("OauthExpires", string.Empty, new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(-1) 
            });
            
            Response.Cookies.Append("OauthHMAC", string.Empty, new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(-1)
            });

            return Redirect($"https://idp.ca.testkontur.ru:8444/realms/Kontur/protocol/openid-connect/logout" +
                                  $"?id_token_hint={idToken}&post_logout_redirect_uri=https://localhost:14100");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
