using Newtonsoft.Json.Linq;
using System;

namespace Microsoft.AspNetCore.Authentication.GitHub
{
    public class GitHubTokenResponse
    {
        public GitHubTokenResponse()
        {

        }
        public GitHubTokenResponse(JObject response)
        {
            this.AccessToken = response.Value<string>("access_token");
            this.Scope = response.Value<string>("scope");
            this.TokenType = response.Value<string>("token_type");
        }
        private GitHubTokenResponse(Exception error)
        {
            Error = error;
        }
        public static GitHubTokenResponse Success(JObject response)
        {
            return new GitHubTokenResponse(response);
        }

        public static GitHubTokenResponse Failed(Exception error)
        {
            return new GitHubTokenResponse(error);
        }
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
        public Exception Error { get; set; }
    }
}
