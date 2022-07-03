using Microsoft.AspNetCore.Mvc;
using SelfModifyingCode.Server.DataContract;
using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgramController : ControllerBase
{
    private IProgramRepository ProgramRepository { get; }

    public ProgramController(IProgramRepository programRepository)
    {
        ProgramRepository = programRepository;
    }
    
    [HttpGet]
    public ActionResult<ApiProgramDirectory> Get()
    {
        var directory = ProgramRepository.GetProgramDirectory();
        return Ok(directory.IntoApiFormat());
    }

    [HttpGet("{ProgramName}")]
    public ActionResult<ApiProgramInformation> GetSingle(string programName)
    {
        var program = ProgramRepository.GetProgramByName(programName);
        if (program == null)
        {
            return NotFound();
        }

        return Ok(program.IntoApiFormat());
    }
}