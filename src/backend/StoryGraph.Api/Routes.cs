namespace StoryGraph.Api;

internal static class Routes
{
    private const string Root = "api";
    
    internal static class User
    {
        private const string Base = $"{Root}/user";

        public const string SignUp = $"{Base}/signup";
        public const string SignIn = $"{Base}/signin";
        public const string Delete = $"{Base}";
    }

    internal static class Story
    {
        private const string Base = $"{Root}/story";
        
        public const string Get = $"{Base}";
        public const string Delete = $"{Base}";
    }
}