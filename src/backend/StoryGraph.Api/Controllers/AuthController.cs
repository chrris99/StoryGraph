using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using StoryGraph.Application.Contracts;
using StoryGraph.Application.Repositories;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Abstractions;

namespace StoryGraph.Api.Controllers;

[ApiController]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly ITokenProvider _tokenProvider;
    
    public AuthController(IUserRepository repository, ITokenProvider tokenProvider)
    {
        _repository = repository;
        _tokenProvider = tokenProvider;
    }
    
    [HttpPost]
    [Route(Routes.Auth.Register)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var createResult = await _repository.RegisterAsync(request.Name, request.Email, request.Password);
        
        if (createResult.IsFailure)
            return BadRequest(createResult.Error);

        var user = createResult.Value;

        var token = _tokenProvider.Create(user);

        return Ok(new AuthResponse
        {
            Email = user.Email,
            Name = user.UserName,
            Token = token
        });
    }
    
    [HttpPost]
    [Route(Routes.Auth.Login)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginResult = await _repository.LoginAsync(request.Email, request.Password);
        
        if (loginResult.IsFailure)
            return BadRequest(loginResult.Error);

        var user = loginResult.Value;

        var token = _tokenProvider.Create(user);
        
        return Ok(new AuthResponse
        {
            Email = user.Email,
            Name = user.UserName,
            Token = token
        });
    }
}