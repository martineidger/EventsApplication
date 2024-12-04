using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace Events.Authentications
{
    public static class Config
    {
        public const string Admin = "admin";
        public const string User = "user";

        public static IEnumerable<IdentityResource> IdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("read", "Read your data"),
                new ApiScope("write", "Write your data"),
                new ApiScope("delete", "Delete your data")
            };

        public static IEnumerable<ApiResource> ApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("events_api", "Events API")
                {
                    Scopes = { "read", "write", "delete"} // Убедитесь, что все области указаны
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "events_api",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    ClientSecrets = { new Secret(config.GetValue<string>("ApiSettings:Secret").Sha256()) },
                    RedirectUris = { "https://localhost:7230/account/callback" },
                    PostLogoutRedirectUris = { "https://localhost:7230/" },
                    AllowedScopes =
                    {
                        "read",
                        "write",
                        "delete"
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600,
                    AbsoluteRefreshTokenLifetime = 2592000
                }
            };

    }
}



