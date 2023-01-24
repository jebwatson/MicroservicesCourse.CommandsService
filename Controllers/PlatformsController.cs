using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/commands/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
  public PlatformsController()
  {
      
  }

  [HttpPost]
  public ActionResult TestInboundConnection()
  {
    System.Console.WriteLine("--> Inbound POST # Command Service");

    return Ok("Inbound test ok from Platforms Controller");
  }
}
