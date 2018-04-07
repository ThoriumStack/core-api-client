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
	    private string _tokenBaseUrl;

	    private Dictionary<string, string> Headers { get; set; }
	    private Dictionary<string, string> TokenHeaders { get; set; }

		public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context, Dictionary<string, string> headers)
		{
			_context = context;
			_tokenAuthenticationCredentials = tokenAuthenticationCredentials;
			_baseUrl = baseUrl;
			Headers = headers;
			_tokenBaseUrl = _baseUrl;
			
//			FlurlHttp.Configure(settings => {
//				settings.HttpClientFactory = new ReallyNaughtyHttpClientFactory();
//			});

			_tokenStore = new DefaultTokenStore();

		}

	    public bool AddHostHeaders { get; set; } = true;

		public MyBucksApiClient(string baseUrl, TokenAuthenticationCredentials tokenAuthenticationCredentials, string context)
		{
			_context = context;
			
			_tokenAuthenticationCredentials = tokenAuthenticationCredentials;
			_baseUrl = baseUrl;
			_tokenBaseUrl = _baseUrl;
//			FlurlHttp.Configure(settings => {
//				settings.HttpClientFactory = new ReallyNaughtyHttpClientFactory();
//			});
			
			_tokenStore = new DefaultTokenStore();
		}

	    public void SetTokenBaseUrl(string tokenBaseUrl) 
	    {
		    _tokenBaseUrl = tokenBaseUrl;
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
			if (TokenHeaders == null) TokenHeaders = new Dictionary<string, string>();

			if (!TokenHeaders.ContainsKey("Host")) TokenHeaders.Add("Host", "fincloud.getbucks.com");
	       // if (!_headers.ContainsKey("X-Forwarded-Proto")) _headers.Add("X-Forwarded-Proto", "https");
	        if (_tokenStore.GetToken() != null)
	        {
		        return _tokenStore.GetToken();
	        }
			var result = await _tokenBaseUrl
                .AppendPathSegment("tokens")
                .AppendPathSegment(_tokenStore.GetToken().RefreshToken)
				.WithHeaders(TokenHeaders)
				.WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(new {context = _context}).ReceiveJson<BearerToken>();
            _tokenStore.SetToken(result);
            return result;
        }

        public async Task<BearerToken> GetAuthToken(string email, string password)
        {
	       
	        
			if (TokenHeaders == null) TokenHeaders = new Dictionary<string, string>();

			if (!TokenHeaders.ContainsKey("Host")) TokenHeaders.Add("Host", "fincloud.getbucks.com");
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
            var result = await _tokenBaseUrl
                .AppendPathSegment("tokens")
				.WithHeaders(Headers)
                .WithBasicAuth(_tokenAuthenticationCredentials.ClientId, _tokenAuthenticationCredentials.ClientSecret)
                .PostJsonAsync(accountModel)
                .ReceiveJson<BearerToken>();
	        _tokenStore.SetToken(result);
            return result;
        }

        public IFlurlRequest GetRequest()
        {
	        if (!Headers.ContainsKey("Host") && AddHostHeaders) Headers.Add("Host", "fincloud.getbucks.com");
	        
            var token = _tokenStore.GetToken();
            if (token == null)
            {
                throw new Exception("Logged out");
            }

            return _baseUrl.WithOAuthBearerToken(token.AccessToken).WithHeaders(Headers);
        }
    }
}