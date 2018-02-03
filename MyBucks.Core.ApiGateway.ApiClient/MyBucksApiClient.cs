using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MyBucks.Core.ApiGateway.ApiClient.Models;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    public class MyBucksApiClient
    {
        protected readonly TokenAuthenticationCredentials TokenAuthenticationCredentials;

        private readonly string _context;
        private BearerToken _tokenCollection;
        private readonly string _baseUrl;

        public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context)
        {
            _context = context;
            TokenAuthenticationCredentials = tokenAuthenticationCredentials;
            _baseUrl = baseUrl;
        }

        public async Task<BearerToken> RefreshToken(BearerToken existingToken)
        {
            var result = await _baseUrl
                .AppendPathSegment("tokens")
                .AppendPathSegment(existingToken.RefreshToken)
                .WithBasicAuth(TokenAuthenticationCredentials.ClientId, TokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(new {_context}).ReceiveJson<BearerToken>();
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
                .WithBasicAuth(TokenAuthenticationCredentials.ClientId, TokenAuthenticationCredentials.ClientSecret)
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