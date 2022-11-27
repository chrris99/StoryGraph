using Microsoft.AspNetCore.Identity;

using StoryGraph.Application.Repositories;
using StoryGraph.Domain.Abstractions;
using StoryGraph.Domain.Errors;

namespace StoryGraph.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IdentityUser>> RegisterAsync(string email, string name, string password)
    {
        var userName = string.IsNullOrWhiteSpace(name) ? email : name;
        
        var user = new IdentityUser
        {
            Email = email,
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);

        var error = result.Errors.FirstOrDefault(e => e.Code.EndsWith(DomainErrors.User.DuplicateEmail.Message)) != null
            ? DomainErrors.User.DuplicateEmail
            : DomainErrors.User.CreateFailed;
        
        return result.Succeeded
            ? user
            : Result.Failure<IdentityUser>(error);
    }

    public async Task<Result<IdentityUser>> LoginAsync(string email, string password)
    {
        var getUserResult = await GetByEmailAsync(email);

        if (getUserResult.IsFailure)
            return Result.Failure<IdentityUser>(getUserResult.Error);

        var user = getUserResult.Value;

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

        return isPasswordCorrect
            ? user
            : Result.Failure<IdentityUser>(DomainErrors.User.IncorrectPassword);
    }
    
    private async Task<Result<IdentityUser>> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        return user ?? Result.Failure<IdentityUser>(DomainErrors.User.NotFoundById);
    }

    private async Task<Result<IdentityUser>> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user ?? Result.Failure<IdentityUser>(DomainErrors.User.NotFoundByEmail);
    }

    private async Task<Result<IdentityUser>> GetByNameAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);

        return user ?? Result.Failure<IdentityUser>(DomainErrors.User.NotFoundByName);
    }
}