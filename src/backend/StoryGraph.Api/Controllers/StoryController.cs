using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using StoryGraph.Application.Contracts;
using StoryGraph.Application.Repositories;
using StoryGraph.Application.Services;

namespace StoryGraph.Api.Controllers;

[ApiController]
public sealed class StoryController : ControllerBase
{
    private readonly IStoryRepository _repository;
    private readonly IVisualizationService _visualizationService;
    
    public StoryController(IStoryRepository repository, IVisualizationService visualizationService)
    {
        _repository = repository;
        _visualizationService = visualizationService;
    }
    
    [HttpPost]
    [Route(Routes.Story.Create)]
    public async Task<IActionResult> Create([FromBody] StoryRequest request)
    {
        var result = await _visualizationService.VisualizeAsync(request.Text);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        /*
        if (HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
        {
            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            _repository.CreateAsync(story);
        }*/
        
        return Ok();
    }
    
    [HttpGet]
    [Route(Routes.Story.Get)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
    {
        return Ok();
    }
    
    [HttpDelete]
    [Route(Routes.Story.Delete)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Delete(string id)
    {
        return Ok();
    }
}