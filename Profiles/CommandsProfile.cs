using AutoMapper;
using CommandService.Dtos;
using CommandsService.Models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
    }
}
