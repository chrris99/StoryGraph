using StoryGraph.Validation.Domain.Abstractions;

namespace StoryGraph.Validation.Domain.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static class DomainErrors
{
    public static class Title
    {
        public static Error Empty =>
            new ("Validation.Title.Empty", "Title is empty.");
        
        public static Error MaxLengthExceeded =>
            new("Validation.Title.MaxLengthExceeded", "Title exceeded maximum length.");
    }
    
    public static class Text
    {
        public static Error Empty =>
            new ("Validation.Text.Empty", "Text is empty.");
        
        public static Error MaxLengthExceeded =>
            new("Validation.Text.MaxLengthExceeded", "Text exceeded maximum length.");
    }
}