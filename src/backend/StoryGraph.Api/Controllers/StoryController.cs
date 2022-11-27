using Microsoft.AspNetCore.Mvc;

namespace StoryGraph.Api.Controllers;

[ApiController]
public sealed class StoryController : ControllerBase
{
    [HttpGet]
    [Route(Routes.Story.Get)]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpDelete]
    [Route(Routes.Story.Delete)]
    public IActionResult Delete()
    {
        return Ok();
    }
}