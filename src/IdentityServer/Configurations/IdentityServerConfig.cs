using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer.Configurations
{
    public class IdentityServerConfig
    {
        public static readonly List<ApiResource> ApiResources = new List<ApiResource>()
        {
            new ApiResource("ApiOne"),
            new ApiResource("ApiTwo")
        };
        
        public static readonly List<ApiScope> ApiScopes = new List<ApiScope>()
        {
            new ApiScope("ApiOne"),
            new ApiScope("TopSecret")
        };

        public static readonly List<IdentityResource> IdentityResources = new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("userClaims", new string[]
            {
                "Developer",
                "Scientist"
            })
        };

        public static readonly List<Client> Clients = new List<Client>()
        {
            // Client credentials flow
            new Client
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"ApiOne"}
            },
            new Client
            {
                ClientId = "client_id_top_secret",
                ClientSecrets = { new Secret("client_secret_top_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"ApiOne", "TopSecret"}
            },
            // Authorization code flow
            new Client
            {
                AllowedGrantTypes = GrantTypes.Code,
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256())},
                RedirectUris = { "https://localhost:8001/signin-oidc" },
                PostLogoutRedirectUris = {"https://localhost:8001/signout-callback-oidc"},
                AllowedScopes = { "openid", "profile", "userClaims" }
            }
        };
    }
}