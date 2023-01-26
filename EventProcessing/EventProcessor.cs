using System.Text.Json;
using AutoMapper;
using CommandService.Dtos;
using CommandsService.Data;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        this.scopeFactory = scopeFactory;
        this.mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        System.Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published":
                System.Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                System.Console.WriteLine("--> Could not determine event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var platform = mapper.Map<Platform>(platformPublishedDto);

            if (repo.ExternalPlatformExists(platform.ExternalId)) return;

            repo.CreatePlatform(platform);
            repo.SaveChanges();

            System.Console.WriteLine("--> Platform Added!");
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"--> Could not add Platform to DB: {ex.Message}");
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
