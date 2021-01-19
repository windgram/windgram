using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.GitHub
{
    public class GitHubHandler : RemoteAuthenticationHandler<GitHubOptions>
    {
        private HttpClient Backchannel => Options.Backchannel;

        /// <summary>
        /// The handler calls methods on the events which give the application control at certain points where processing is occurring. 
        /// If it is not provided a default instance is supplied which does nothing when the methods are called.
        /// </summary>
        protected new GitHubEvents Events
        {
            get { return (GitHubEvents)base.Events; }
            set { base.Events = value; }
        }

        /// <summary>
        /// Creates a new instance of the events instance.
        /// </summary>
        /// <returns>A new instance of the events instance.</returns>
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new GitHubEvents());

        public GitHubHandler(IOptionsMonitor<GitHubOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
            }

            // OAuth2 10.12 CSRF
            GenerateCorrelationId(properties);

            var authorizationEndpoint = BuildChallengeUrl(properties, BuildRedirectUri(Options.CallbackPath));
            var redirectContext = new RedirectContext<GitHubOptions>(
                Context, Scheme, Options,
                properties, authorizationEndpoint);
            await Events.RedirectToAuthorizationEndpoint(redirectContext);

            var location = Context.Response.Headers[HeaderNames.Location];
            if (location == StringValues.Empty)
            {
                location = "(not set)";
            }
            var cookie = Context.Response.Headers[HeaderNames.SetCookie];
            if (cookie == StringValues.Empty)
            {
                cookie = "(not set)";
            }
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query;
            var code = query["code"];
            var state = query["state"];
            var properties = Options.StateDataFormat.Unprotect(state);
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.");
            }
            if (string.IsNullOrEmpty(code))
            {
                return HandleRequestResult.Fail("Missing code", properties);
            }
            var redirectUri = BuildRedirectUri(Options.CallbackPath);
            var tokenResponse = await this.ExchangeCodeAsync(code, state, redirectUri);
            if (tokenResponse.Error != null)
            {
                return HandleRequestResult.Fail(tokenResponse.Error);
            }
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return HandleRequestResult.Fail("Missing access token.");
            }
            var identity = new ClaimsIdentity(ClaimsIssuer);
            var ticket = await CreateTicketAsync(identity, properties, tokenResponse);
            return HandleRequestResult.Success(ticket);
        }
        protected virtual async Task<GitHubTokenResponse> ExchangeCodeAsync(string code, string state, string redirectUri)
        {
            var parameters = new SortedDictionary<string, string>
            {
                {  "client_id", Options.ClientId },
                {  "client_secret", Options.ClientSecret },
                {  "code", code},
                {  "redirect_uri", redirectUri },
                {  "state", state }
            };
            var endpoint = QueryHelpers.AddQueryString(Options.TokenEndpoint, parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Add("Accept", "application/json");
            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                return GitHubTokenResponse.Success(payload);
            }
            else
            {
                return GitHubTokenResponse.Failed(new Exception("Exchange Code for access token failed"));
            }
        }
        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, GitHubTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Microsoft user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Microsoft Account API is enabled.");
            }
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.Value<string>("id")))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, payload.Value<string>("id"), ClaimValueTypes.String, ClaimsIssuer));
            }
            if (!string.IsNullOrEmpty(payload.Value<string>("name")))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, payload.Value<string>("name"), ClaimValueTypes.String, ClaimsIssuer));
            }
            if (!string.IsNullOrEmpty(payload.Value<string>("avatar_url")))
            {
                identity.AddClaim(new Claim("picture", payload.Value<string>("avatar_url"), ClaimValueTypes.String, ClaimsIssuer));
            }
            if (!string.IsNullOrEmpty(payload.Value<string>("email")))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, payload.Value<string>("email"), ClaimValueTypes.String, ClaimsIssuer));
            }
            if (!string.IsNullOrEmpty(payload.Value<string>("html_url")))
            {
                identity.AddClaim(new Claim(ClaimTypes.Webpage, payload.Value<string>("html_url"), ClaimValueTypes.String, ClaimsIssuer));
            }
            var context = new GitHubCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload);
            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);

        }

        protected virtual string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "allow_signup", Options.AllowSignup.ToString().ToLower() }
            };
            if (Options.Scope != null && Options.Scope.Any())
            {
                queryStrings.Add("scope", string.Join(" ", Options.Scope));
            }
            var state = Options.StateDataFormat.Protect(properties);
            queryStrings.Add("state", state);
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryStrings);
        }
    }
}
