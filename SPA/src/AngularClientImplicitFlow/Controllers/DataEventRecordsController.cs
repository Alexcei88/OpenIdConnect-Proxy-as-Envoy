using System.Linq;
using System.Threading.Tasks;
using Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class DataEventRecordsController : Controller
{
    private readonly ResourceServerClient _client;

    public DataEventRecordsController(ResourceServerClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<DataEventRecord[]> Get()
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        return (await _client.DataEventRecordsAllAsync()).ToArray();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DataEventRecord>> Get(long id)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        return await _client.DataEventRecordsGETAsync(id);
    }

    [HttpPost]
    public async Task<DataEventRecord> Post([FromBody] DataEventRecord value)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        return await _client.DataEventRecordsPOSTAsync(value);
    }

    [HttpPut("{id}")]
    public async Task Put(long id, [FromBody] DataEventRecord value)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        await _client.DataEventRecordsPUTAsync(id, value);
    }

    [HttpDelete("{id}")]
    public async Task Delete(long id)
    {
        _client.SetBearerToken(HttpContext.Items["AccessToken"].ToString());
        await _client.DataEventRecordsDELETEAsync(id);
    }    
}