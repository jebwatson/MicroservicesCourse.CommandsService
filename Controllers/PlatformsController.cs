using AutoMapper;
using CommandService.Dtos;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/commands/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo repo;
    private readonly IMapper mapper;

    public PlatformsController(ICommandRepo repo, IMapper mapper)
    {
        this.repo = repo;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      System.Console.WriteLine("--> Getting Platforms from CommandService");

      var platforms = repo.GetPlatforms();

      return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        System.Console.WriteLine("--> Inbound POST # Command Service");

        return Ok("Inbound test ok from Platforms Controller");
    }
}
