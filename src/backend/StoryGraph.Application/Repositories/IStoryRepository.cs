using StoryGraph.Domain.Abstractions;
using StoryGraph.Domain.Entities;

namespace StoryGraph.Application.Repositories;

public interface IStoryRepository
{
    Task<Result> CreateAsync(Story story);
    
    Task<IEnumerable<Story>> GetByUserAsync(string email);

    Task<Result> DeleteByIdAsync(string id);
}