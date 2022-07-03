using Microsoft.AspNetCore.Mvc;

namespace SelfModifyingCode.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{

    [HttpGet("{ProgramId}")]
    public ActionResult Get(string programId)
    {
        return Ok();
    }
    
}