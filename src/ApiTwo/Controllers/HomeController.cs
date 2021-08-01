using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiTwo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        [HttpGet("/home")]
        public async Task<IActionResult> Index()
        {
            var identityServerUrl = _configuration["AuthenticationConfig:Authority"];
            var apiOneUrl = _configuration["ApiOne:SecretUrl"];
            var apiOneTopSecretUrl = _configuration["ApiOne:TopSecretUrl"];
            var client = _clientFactory.CreateClient();
            var discovery = await client.GetDiscoveryDocumentAsync(identityServerUrl);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = _configuration["Client:ClientId"],
                ClientSecret = _configuration["Client:ClientSecret"],
                Scope = _configuration["Client:Scopes"]
            });
            
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync(apiOneTopSecretUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            return BadRequest("Request failed. response code: " + response.StatusCode);
        }
        
        [HttpGet("/TopSecret")]
        public async Task<IActionResult> TopSecret()
        {
            var identityServerUrl = _configuration["AuthenticationConfig:Authority"];
            var apiOneUrl = _configuration["ApiOne:SecretUrl"];
            var apiOneTopSecretUrl = _configuration["ApiOne:TopSecretUrl"];
            var client = _clientFactory.CreateClient();
            var discovery = await client.GetDiscoveryDocumentAsync(identityServerUrl);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = _configuration["TopSecretClient:ClientId"],
                ClientSecret = _configuration["TopSecretClient:ClientSecret"],
                Scope = _configuration["TopSecretClient:Scopes"]
            });
            
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync(apiOneTopSecretUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            return BadRequest("Request failed. response code: " + response.StatusCode);
        }
    }
}