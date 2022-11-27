using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using StoryGraph.Domain.Entities;

namespace StoryGraph.Infrastructure;

public sealed class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Story> Stories { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}