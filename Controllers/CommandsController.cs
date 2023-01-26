using AutoMapper;
using CommandService.Dtos;
using CommandsService.Data;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/commands/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo repo;
    private readonly IMapper mapper;

    public CommandsController(ICommandRepo repo, IMapper mapper)
    {
        this.repo = repo;
        this.mapper = mapper;
    }

    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        System.Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

        if (!repo.PlatformExists(platformId)) return NotFound();

        var commands = repo.GetCommandsForPlatform(platformId);

        return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        System.Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        if (!repo.PlatformExists(platformId)) return NotFound();

        var command = repo.GetCommand(platformId, commandId);

        return command is not null ?
          Ok(mapper.Map<CommandReadDto>(command)) :
          NotFound();
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        System.Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!repo.PlatformExists(platformId)) return NotFound();

        var command = mapper.Map<Command>(commandDto);

        repo.CreateCommand(platformId, command);
        repo.SaveChanges();

        var commandReadDto = mapper.Map<CommandReadDto>(command);
        return CreatedAtRoute(
            nameof(GetCommandForPlatform),
            new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }
}
