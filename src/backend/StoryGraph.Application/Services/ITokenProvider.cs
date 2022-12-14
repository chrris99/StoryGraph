using Microsoft.AspNetCore.Identity;

namespace StoryGraph.Application.Services;

/// <summary>
/// Represents an authentication token provider service.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// Create an authentication token for the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The created token for the user.</returns>
    string Create(IdentityUser user);
}