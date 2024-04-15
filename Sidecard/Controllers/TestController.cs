using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Sidecard.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/")]
public class TestController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TestController> _logger;
    public TestController(IConfiguration configuration, ILogger<TestController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    [HttpPost("/actions/hello")]
    public IActionResult Hello()
    {
        return Ok(new { response = "Hello from c#" });
    }

    [HttpPost("/events/sample.event")]
    public IActionResult SampleEvent()
    {
        _logger.LogInformation("Sample event triggered");
        return Ok("OK");
    }
}