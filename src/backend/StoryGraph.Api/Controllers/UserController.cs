using Microsoft.AspNetCore.Mvc;

namespace StoryGraph.Api.Controllers;

[ApiController]
public sealed class UserController : ControllerBase
{
    [HttpPost]
    [Route(Routes.User.SignUp)]
    public IActionResult SignUp()
    {
        return Ok();
    }

    [HttpPost]
    [Route(Routes.User.SignIn)]
    public IActionResult SignIn()
    {
        return Ok();
    }

    [HttpDelete]
    [Route(Routes.User.Delete)]
    public IActionResult Delete()
    {
        return Ok();
    }
}