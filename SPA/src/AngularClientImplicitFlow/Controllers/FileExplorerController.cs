using System.Collections.Generic;
using System.Threading.Tasks;
using FileServer.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ResourceFileServer.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class FileExplorerController : Controller
{
    private readonly ResourceFileServerClient _client;

    public FileExplorerController(ResourceFileServerClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<ICollection<string>> Get()
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        return await _client.FileExplorerAsync();
    }
}