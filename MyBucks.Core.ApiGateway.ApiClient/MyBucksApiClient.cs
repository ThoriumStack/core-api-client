using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MyBucks.Core.ApiGateway.ApiClient.Models;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    public class MyBucksApiClient
    {
        private readonly TokenAuthenticationCredentials _tokenAuthenticationCredentials;

        private readonly string _context;
        private BearerToken _tokenCollection;
        private readonly string _baseUrl;

        public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context)
        {
            _context = context;
            _tokenAuthenticationCredentials = tokenAuthenticationCredentials;
            _baseUrl = baseUrl;
        }

        public void SetToken(BearerToken existingToken)
        {
            _tokenCollection = existingToken;
        }

        public async Task<BearerToken> RefreshToken()
        {
            var result = await _baseUrl
                .AppendPathSegment("tokens")
                .AppendPathSegment(_tokenCollection.RefreshToken)
                .WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(new {context = _context}).ReceiveJson<BearerToken>();
            _tokenCollection = result;
            return result;
        }

        public async Task<BearerToken> GetAuthToken(string email, string password)
        {
            var accountModel = new UserAuthenticationRequest
            {
                Context = _context,
                Email = email,
                Password = password,
                MobileNumber = ""
            };
            var result = await _baseUrl
                .AppendPathSegment("tokens")
                .WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(accountModel)
                .ReceiveJson<BearerToken>();
            _tokenCollection = result;
            return result;
        }

        public IFlurlRequest GetRequest()
        {
            var token = _tokenCollection;
            if (token == null)
            {
                throw new Exception("Logged out");
            }

            return _baseUrl.WithOAuthBearerToken(token.AccessToken);
        }
    }
}