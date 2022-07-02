using Microsoft.AspNetCore.Mvc;

namespace BasicWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EchoController : ControllerBase
{
    
    private readonly ILogger<EchoController> _logger;

    public EchoController(ILogger<EchoController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public ActionResult<string> Get()
    {
        var name = "Guy";
        _logger.LogInformation("Got echo request for '{Name}'", name);
        var pingString = $"Hello, '{name}'";
        return pingString;
    }

    
}