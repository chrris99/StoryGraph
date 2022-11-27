using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using StoryGraph.Application.Services;

namespace StoryGraph.Infrastructure.Authentication;

/// <summary>
/// Represents a JWT authentication token provider service.
/// </summary>
public sealed class JwtProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtProvider"/> class.
    /// </summary>
    /// <param name="options">The JWT configuration options.</param>
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    /// <inheritdoc />
    public string Create(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Email, user.Email)
            }),
            // TODO: Add expiration date
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}