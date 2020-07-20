using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace MicroserviceArchitecture.GatewayApi.Services
{
    public class TokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private DiscoveryDocumentResponse _discoveryDocument;
        private TokenResponse _tokenResponse;
        private DateTime _tokenExpireTime;

        public TokenService(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
        }

        private async Task DiscoveryDocumentAsync()
        {
            var client = _httpClientFactory.CreateClient();

            _discoveryDocument = await client.GetDiscoveryDocumentAsync("https://localhost:5000");

            if (_discoveryDocument.IsError) throw new Exception(_discoveryDocument.Error);
        }

        private async Task<string> RequestNewTokenAsync()
        {
            await DiscoveryDocumentAsync();

            var client = _httpClientFactory.CreateClient();

            _tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientId = "gateway",
                ClientSecret = "secret"
            });

            if (_tokenResponse.IsError) throw new Exception(_tokenResponse.Error);

            _tokenExpireTime = DateTime.Now.AddHours(2.5);

            return _tokenResponse.AccessToken;
        }

        public async Task<string> GetTokenAsync()
        {
            if (_tokenExpireTime < DateTime.Now)
            {
                return await RequestNewTokenAsync();
            }

            return _tokenResponse.AccessToken;
        }
    }
}
