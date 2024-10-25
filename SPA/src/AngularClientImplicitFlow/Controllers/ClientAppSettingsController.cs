using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using AngularClient.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AngularClient.Controllers
{
    [Route("api/[controller]")]
    public class ClientAppSettingsController : Controller
    {
        private readonly ClientAppSettings _clientAppSettings;

        public ClientAppSettingsController(IOptions<ClientAppSettings> clientAppSettings)
        {
            _clientAppSettings = clientAppSettings.Value;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clientAppSettings);
        }
        
        [HttpGet("username")]
        public IActionResult GetUserName()
        {
            HttpContext.Request.Cookies.TryGetValue("IdToken", out var jwtToken);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);
            
            return Ok(new
            {
                name = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value
            });
        }
        
        
        [HttpGet("logoutUrl")]
        public IActionResult LogoutLink()
        {
            HttpContext.Request.Cookies.TryGetValue("IdToken", out var idToken);
            return Ok(new
            {
                url = $"https://idp.ca.testkontur.ru:8444/realms/Kontur/protocol/openid-connect/logout" +
                $"?id_token_hint={idToken}&post_logout_redirect_uri=https://localhost:14100"
            });
        }
    }
}
