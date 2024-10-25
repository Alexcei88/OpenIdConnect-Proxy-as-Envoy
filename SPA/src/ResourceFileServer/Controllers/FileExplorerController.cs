using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ResourceFileServer.Providers;

namespace ResourceFileServer.Controllers;

[Authorize("securedFiles")]
[Route("api/[controller]")]
public class FileExplorerController : Controller
{
    private readonly ISecuredFileProvider _securedFileProvider;

    public FileExplorerController(ISecuredFileProvider securedFileProvider)
    {
        _securedFileProvider = securedFileProvider;
    }

    //[Authorize("securedFilesUser")]
    [HttpGet]
    public ActionResult<string[]> Get()
    {
        var files = _securedFileProvider.GetFilesForUser(true);
        return Ok(files);
    }
}