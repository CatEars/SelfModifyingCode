using Microsoft.AspNetCore.Mvc;
using SelfModifyingCode.Server.DataContract;

namespace SelfModifyingCode.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgramController : ControllerBase
{

    [HttpGet]
    public ActionResult<ApiProgramDirectory> Get()
    {
        return Ok(new ApiProgramDirectory(new List<ApiProgramInformation>()));
    }

    [HttpGet("{ProgramName}")]
    public ActionResult<ApiProgramInformation> GetSingle(string programName)
    {
        return Ok(new ApiProgramInformation(
            programName, 
            "v1.0", 
            new ApiIdentity(ApiIdentityVersion.Sha256, "abc"),
            $"{programName}.smc",
            $"/api/Download/{programName}"));
    }
}