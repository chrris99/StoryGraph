using Microsoft.EntityFrameworkCore;

using StoryGraph.Application.Repositories;
using StoryGraph.Domain.Abstractions;
using StoryGraph.Domain.Entities;
using StoryGraph.Domain.Errors;

namespace StoryGraph.Infrastructure.Repositories;

public class StoryRepository : IStoryRepository
{
    private readonly ApplicationDbContext _context;
    
    public StoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> CreateAsync(Story story)
    {
        await _context
            .Stories
            .AddAsync(story);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<IEnumerable<Story>> GetByUserAsync(string email)
    {
        return await _context
            .Stories
            .Where(story => story.CreatedBy.Equals(email))
            .ToListAsync();
    }

    public async Task<Result> DeleteByIdAsync(string id)
    {
        var story = await FindByIdAsync(id);
        
        if (story.IsFailure) 
            return Result.Success();

        _context
            .Stories
            .Remove(story.Value);

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result<Story>> FindByIdAsync(string id)
    {
        var story = await _context
            .Stories
            .FirstOrDefaultAsync(s => s.Id.Equals(id));

        return story ?? Result.Failure<Story>(DomainErrors.Story.NotFoundById);
    }
}