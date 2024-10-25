using ResourceServer.Model;
using ResourceServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResourceServer.Controllers;

[Authorize("dataEventRecords")]
[Route("api/[controller]")]
public class DataEventRecordsController : Controller
{
    private readonly IDataEventRecordRepository _dataEventRecordRepository;

    public DataEventRecordsController(IDataEventRecordRepository dataEventRecordRepository)
    {
        _dataEventRecordRepository = dataEventRecordRepository;
    }

    [Authorize("dataEventRecordsUser")]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_dataEventRecordRepository.GetAll());
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        return Ok(_dataEventRecordRepository.Get(id));
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpPost]
    public ActionResult<DataEventRecord> Post([FromBody] DataEventRecord value)
    {
        if (value == null)
            return Ok();
        _dataEventRecordRepository.Post(value);
        return Ok(value);
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpPut("{id}")]
    public void Put(long id, [FromBody] DataEventRecord value)
    {
        _dataEventRecordRepository.Put(id, value);
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpDelete("{id}")]
    public void Delete(long id)
    {
        _dataEventRecordRepository.Delete(id);
    }
}