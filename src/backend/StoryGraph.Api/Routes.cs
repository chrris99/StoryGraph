namespace StoryGraph.Api;

internal static class Routes
{
    private const string Root = "api";
    
    internal static class Auth
    {
        private const string Base = $"{Root}/auth";

        public const string Login = $"{Base}/login";
        public const string Register = $"{Base}/register";
    }

    internal static class Story
    {
        private const string Base = $"{Root}/story";

        public const string Create = $"{Base}";
        public const string Get = $"{Base}";
        public const string Delete = Base + "/{id:guid}";
    }
}