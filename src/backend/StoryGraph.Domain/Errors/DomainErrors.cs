using StoryGraph.Domain.Abstractions;

namespace StoryGraph.Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error CreateFailed =>
            new("User.CreateFailed", "Unknown error occured when creating a new user.");

        public static Error NotFoundById =>
            new Error("User.NotFoundById", "User with given ID not found in database.");

        public static Error NotFoundByEmail =>
            new("User.NotFoundByEmail", "User with given email not found in database.");

        public static Error NotFoundByName =>
            new("User.NotFoundByName", "User with given name not found in database");

        public static Error DuplicateEmail =>
            new("User.DuplicateEmail", "Email already in use. Try signing in instead.");

        public static Error IncorrectPassword =>
            new("User.IncorrectPassword", "The provided password is incorrect.");
    }

    public static class Story
    {
        public static Error NotFoundById =>
            new("Story.NotFoundById", "Story with given ID not found in database.");
    }
}