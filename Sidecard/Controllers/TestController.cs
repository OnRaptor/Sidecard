using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Sidecard.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("/")]
public class TestController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public TestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("/actions/hello")]
    public IActionResult Hello()
    {
        return Ok(new { response = "Hello from c#" });
    }
}