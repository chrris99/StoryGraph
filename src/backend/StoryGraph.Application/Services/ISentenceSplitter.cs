namespace StoryGraph.Application.Services;

public interface ISentenceSplitter
{
    Task<IEnumerable<string>> SplitAsync(string text);
}