using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StoryGraph.Validation.Domain;

namespace StoryGraph.Validation.Web.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ValidationController : ControllerBase
{
    private readonly IValidator<Story> _validator;
    
    public ValidationController(IValidator<Story> validator)
    {
        _validator = validator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Validate([FromBody] Story story)
    {
        var result = await _validator.ValidateAsync(story);

        if (result.IsValid) return Ok();
        
        var errorAggregate =
            result.Errors.Aggregate(string.Empty, (current, error) => $"{current}\n{error.ErrorMessage}");

        return BadRequest(errorAggregate);
    }
}