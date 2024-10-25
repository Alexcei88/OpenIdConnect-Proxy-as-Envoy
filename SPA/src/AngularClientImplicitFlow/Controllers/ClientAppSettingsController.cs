using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers
{
    [Route("[controller]")]
    public class ClientAppSettingsController : ControllerBase
    {
        private const string DefaultBackPath = "/";
        
        [HttpGet("login")]
        public async Task Login([FromQuery] string backUrl = DefaultBackPath)
        {
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = backUrl });
        }
        
        [HttpGet("logout")]
        public IActionResult Logout([FromQuery] string backUrl = DefaultBackPath)
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = backUrl
            };
            // after signout this will redirect to your provided target
            //await HttpContext.SignOutAsync("oidc", prop);
            return new SignOutResult(new [] {CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme}, prop);
        }
        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("checkLogin")]
        public IActionResult CheckLogin()
        {
            var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            return Ok(new
            {
                isAuthenticated = true,
                userData = new
                {
                    name = name ?? "Unknown name"
                }
            });
        }
    }
}
