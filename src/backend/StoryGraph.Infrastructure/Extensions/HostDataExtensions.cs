using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StoryGraph.Infrastructure.Extensions;

public static class HostDataExtensions
{
    public static IHost MigrateDatabase<TDbContext>(this IHost host)
        where TDbContext : IdentityDbContext
    {
        using var scope = host.Services.CreateScope();

        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<TDbContext>();
        
        context.Database.Migrate();

        return host;
    }
}