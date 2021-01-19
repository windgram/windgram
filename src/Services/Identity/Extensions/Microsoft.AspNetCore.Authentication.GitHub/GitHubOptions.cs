using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Authentication.GitHub
{
    public class GitHubOptions : RemoteAuthenticationOptions
    {
        /// <summary>
        /// Initializes a new <see cref="GitHubOptions"/>.
        /// </summary>
        public GitHubOptions()
        {
            CallbackPath = new PathString("/signin-github");
            TokenEndpoint = GitHubDefaults.TokenEndpoint;
            AuthorizationEndpoint = GitHubDefaults.AuthorizationEndpoint;
            UserInformationEndpoint = GitHubDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the provider-assigned client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the provider-assigned client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// </summary>
        public string TokenEndpoint { get; set; }
        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// This value is not used in the default implementation, it is for use in custom implementations of
        /// IOAuthAuthenticationEvents.Authenticated or OAuthAuthenticationHandler.CreateTicketAsync.
        /// </summary>
        public string UserInformationEndpoint { get; set; }

        public bool AllowSignup { get; set; }

        /// <summary>
        /// Gets the list of permissions to request.
        /// </summary>
        public ICollection<string> Scope { get; } = new HashSet<string>();
        /// <summary>
        /// Gets or sets the <see cref="GitHubEvents"/> used to handle authentication events.
        /// </summary>
        public new GitHubEvents Events
        {
            get { return (GitHubEvents)base.Events; }
            set { base.Events = value; }
        }
        /// <summary>
        /// Gets or sets the type used to secure data handled by the handler.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        /// A collection of claim actions used to select values from the json user data and create Claims.
        /// </summary>
        public ClaimActionCollection ClaimActions { get; } = new ClaimActionCollection();
    }
}
