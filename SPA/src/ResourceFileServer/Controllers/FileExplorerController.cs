using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ResourceFileServer.Providers;

namespace ResourceFileServer.Controllers;

[Authorize("securedFiles")]
[Route("resourceFileServer/api/[controller]")]
public class FileExplorerController : Controller
{
    private readonly ISecuredFileProvider _securedFileProvider;

    public FileExplorerController(ISecuredFileProvider securedFileProvider)
    {
        _securedFileProvider = securedFileProvider;
    }

    //[Authorize("securedFilesUser")]
    [HttpGet]
    public IActionResult Get()
    {
        var files = _securedFileProvider.GetFilesForUser(true);

        return Ok(files);
    }
}