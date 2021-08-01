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
            new IdentityServer4.Models.IdentityResources.OpenId(),
            new IdentityServer4.Models.IdentityResources.Profile(),
        };

        public static readonly List<Client> Clients = new List<Client>()
        {
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
            }
        };
    }
}