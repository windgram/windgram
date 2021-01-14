using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Windgram.Identity.STS.Configurations
{
    internal class Config
    {
        // ApiResources define the apis in your system
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("filesystem", "FileSystem Service"),
                new ApiScope("message", "Message Service"),
                new ApiScope("user", "User Service"),
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "portal_spa_client",
                    ClientName = "Portal SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = { $"{clientsUrl["PortalSpa"]}/callback/signin", $"{clientsUrl["PortalSpa"]}/callback/silent"  },
                    PostLogoutRedirectUris = { $"{clientsUrl["PortalSpa"]}/callback/logout" },
                    AllowedCorsOrigins = { $"{clientsUrl["PortalSpa"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "filesystem",
                        "message",
                        "user"
                    },
                }
            };
        }
    }
}
