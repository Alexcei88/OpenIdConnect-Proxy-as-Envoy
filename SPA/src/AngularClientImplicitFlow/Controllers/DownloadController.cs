using System.Threading.Tasks;
using FileServer.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ResourceFileServer.Controllers;

[Route("api/[controller]")]
public class DownloadController : Controller
{
    private readonly ResourceFileServerClient _client;

    public DownloadController(ResourceFileServerClient client)
    {
        _client = client;
    }

    [AllowAnonymous]
    [HttpGet("{accessId}/{id}")]
    public async Task<FileContentResult> Get(string accessId, string id)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        byte[] arry = await _client.DownloadAsync(accessId, id);
        return new FileContentResult(arry, "application/octet-stream");
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpGet("GenerateOneTimeAccessToken/{id}")]
    public async Task<DownloadToken> GenerateOneTimeAccessToken(string id)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        return await _client.GenerateOneTimeAccessTokenAsync(id);
    }
}