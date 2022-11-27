using Microsoft.AspNetCore.Identity;
using StoryGraph.Domain.Abstractions;

namespace StoryGraph.Application.Repositories;

public interface IUserRepository
{
    Task<Result<IdentityUser>> RegisterAsync(string email, string name, string password);

    Task<Result<IdentityUser>> LoginAsync(string email, string password);
}