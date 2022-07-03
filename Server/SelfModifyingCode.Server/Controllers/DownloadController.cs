using Microsoft.AspNetCore.Mvc;
using SelfModifyingCode.Server.Directory;

namespace SelfModifyingCode.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{

    private IProgramPathRepository PathRepository { get; }

    public DownloadController(IProgramPathRepository pathRepository)
    {
        PathRepository = pathRepository;
    }
    
    [HttpGet("{ProgramId}")]
    public ActionResult Get(string programId)
    {
        var path = PathRepository.GetPathByProgramId(programId);
        if (path == null)
        {
            return NotFound();
        }

        if (path is PathType.WebUrl url)
        {
            return Redirect(url.Uri.ToString());
        }

        if (path is PathType.LocalFile file)
        {
            var name = Path.GetFileName(file.Path);
            return File(System.IO.File.OpenRead(file.Path), "application/zip", name);
        }

        var typeName = path.GetType().Name;
        throw new NotImplementedException($"Download of type {typeName} is not implemented");
    }
    
}