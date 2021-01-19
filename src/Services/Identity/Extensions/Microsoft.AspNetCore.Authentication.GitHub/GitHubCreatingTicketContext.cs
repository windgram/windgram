using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authentication.GitHub
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class GitHubCreatingTicketContext : ResultContext<GitHubOptions>
    {
        /// <summary>
        /// Initializes a new <see cref="OAuthCreatingTicketContext"/>.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="properties">The <see cref="AuthenticationProperties"/>.</param>
        /// <param name="context">The HTTP environment.</param>
        /// <param name="scheme">The authentication scheme.</param>
        /// <param name="options">The options used by the authentication middleware.</param>
        /// <param name="backchannel">The HTTP client used by the authentication middleware</param>
        /// <param name="tokens">The tokens returned from the token endpoint.</param>
        /// <param name="user">The JSON-serialized user.</param>
        public GitHubCreatingTicketContext(
            ClaimsPrincipal principal,
            AuthenticationProperties properties,
            HttpContext context,
            AuthenticationScheme scheme,
            GitHubOptions options,
            HttpClient backchannel,
            GitHubTokenResponse tokens,
            JObject user)
            : base(context, scheme, options)
        {
            if (backchannel == null)
            {
                throw new ArgumentNullException(nameof(backchannel));
            }

            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            TokenResponse = tokens;
            Backchannel = backchannel;
            User = user;
            Principal = principal;
            Properties = properties;
        }

        /// <summary>
        /// Gets the JSON-serialized user or an empty
        /// <see cref="JObject"/> if it is not available.
        /// </summary>
        public JObject User { get; }

        /// <summary>
        /// Gets the token response returned by the authentication service.
        /// </summary>
        public GitHubTokenResponse TokenResponse { get; }

        /// <summary>
        /// Gets the access token provided by the authentication service.
        /// </summary>
        public string AccessToken => TokenResponse.AccessToken;

        /// <summary>
        /// Gets the backchannel used to communicate with the provider.
        /// </summary>
        public HttpClient Backchannel { get; }

        /// <summary>
        /// Gets the main identity exposed by the authentication ticket.
        /// This property returns <c>null</c> when the ticket is <c>null</c>.
        /// </summary>
        public ClaimsIdentity Identity => Principal?.Identity as ClaimsIdentity;

        public void RunClaimActions() => RunClaimActions(User);

        public void RunClaimActions(JObject userData)
        {
            foreach (var action in Options.ClaimActions)
            {
                action.Run(userData, Identity, Options.ClaimsIssuer ?? Scheme.Name);
            }
        }
    }
}
