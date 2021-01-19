namespace Microsoft.AspNetCore.Authentication.GitHub
{
    /// <summary>
    /// Default values for Github authentication
    /// https://developer.github.com/apps/building-oauth-apps/authorizing-oauth-apps/
    /// </summary>
    public static class GitHubDefaults
    {
        public const string AuthenticationScheme = "GitHub";

        public static readonly string DisplayName = "GitHub";

        public static readonly string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

        public static readonly string TokenEndpoint = "https://github.com/login/oauth/access_token";

        public static readonly string UserInformationEndpoint = "https://api.github.com/user";
    }
}
