using StoryGraph.Domain.Abstractions;

namespace StoryGraph.Application.Services;

public interface IVisualizationService
{
    Task<Result> VisualizeAsync(string text);
    
    // call sentence splitter POST /split_sentences 5002 
    // call token enricher POST /enrich_tokens 5003
    // call named entity extractor POST /extract_person_entities 5004
}