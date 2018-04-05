using System;
using System.Collections.Generic;
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
		//private BearerToken _tokenCollection;
        private string _baseUrl;

	    private ITokenStore _tokenStore;

		private Dictionary<string, string> _headers { get; set; }

		public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context, Dictionary<string, string> headers)
		{
			_context = context;
			_tokenAuthenticationCredentials = tokenAuthenticationCredentials;
			_baseUrl = baseUrl;
			_headers = headers;
			
			FlurlHttp.Configure(settings => {
				settings.HttpClientFactory = new ReallyNaughtyHttpClientFactory();
			});

			_tokenStore = new DefaultTokenStore();

		}

		public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context)
		{
			_context = context;
			
			_tokenAuthenticationCredentials = tokenAuthenticationCredentials;
			_baseUrl = baseUrl;
			FlurlHttp.Configure(settings => {
				settings.HttpClientFactory = new ReallyNaughtyHttpClientFactory();
			});
			
			_tokenStore = new DefaultTokenStore();
		}

	    public void SetTokenStore(ITokenStore tokenStore)
	    {
		    _tokenStore = tokenStore;
	    }

		public void SetToken(BearerToken existingToken)
        {
	        _tokenStore.SetToken(existingToken);
        }

        public async Task<BearerToken> RefreshToken()
        {
			if (_headers == null) _headers = new Dictionary<string, string>();

			if (!_headers.ContainsKey("Host")) _headers.Add("Host", "fincloud.getbucks.com");
	       // if (!_headers.ContainsKey("X-Forwarded-Proto")) _headers.Add("X-Forwarded-Proto", "https");

			var result = await _baseUrl
                .AppendPathSegment("tokens")
                .AppendPathSegment(_tokenStore.GetToken().RefreshToken)
				.WithHeaders(_headers)
				.WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(new {context = _context}).ReceiveJson<BearerToken>();
            _tokenStore.SetToken(result);
            return result;
        }

        public async Task<BearerToken> GetAuthToken(string email, string password)
        {
	       
	        
			if (_headers == null) _headers = new Dictionary<string, string>();

			if (!_headers.ContainsKey("Host")) _headers.Add("Host", "fincloud.getbucks.com");
	       // if (!_headers.ContainsKey("X-Forwarded-Proto")) _headers.Add("X-Forwarded-Proto", "https");

	        
	        if (_tokenStore.GetToken() != null)
	        {
		        return _tokenStore.GetToken();
	        }
	        
	        
			var accountModel = new UserAuthenticationRequest
            {
                Context = _context,
                Email = email,
                Password = password,
                MobileNumber = ""
            };
            var result = await _baseUrl
                .AppendPathSegment("tokens")
				.WithHeaders(_headers)
                .WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(accountModel)
                .ReceiveJson<BearerToken>();
	        _tokenStore.SetToken(result);
            return result;
        }

        public IFlurlRequest GetRequest()
        {
	        if (!_headers.ContainsKey("Host")) _headers.Add("Host", "fincloud.getbucks.com");
	        
            var token = _tokenStore.GetToken();
            if (token == null)
            {
                throw new Exception("Logged out");
            }

            return _baseUrl.WithOAuthBearerToken(token.AccessToken).WithHeaders(_headers);
        }
    }
}