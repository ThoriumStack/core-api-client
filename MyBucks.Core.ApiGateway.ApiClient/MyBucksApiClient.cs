using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MyBucks.Core.ApiGateway.ApiClient.Models;
using Thorium.FluentDefense;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    public class MyBucksApiClient
    {
	    private MyBucksApiOptions _options;
	    
	    private Dictionary<string, string> TokenHeaders { get; set; } = new Dictionary<string, string>();

	    public MyBucksApiClient()
	    {
		    _options = new MyBucksApiOptions {TokenStore = new DefaultTokenStore()};
	    }

	    public MyBucksApiClient Configure(string baseUrl, string context, string userId, int timeZoneOffSetInMinutes=0, Action<MyBucksApiOptions> setupAction=null)
	    {
		    _options.WithContext(context);
		    _options.WithUserId(userId);
		    _options.WithTimeZoneOffset(timeZoneOffSetInMinutes);
		    _options.BaseUrl = baseUrl;
		    setupAction?.Invoke(_options);
		    return this;
	    }

        public async Task<BearerToken> RefreshToken()
        {
	        if (!_options.HasAuthentication)
	        {
		        throw new Exception("Cannot refresh token. No authentication details specified.");
	        }

	        var (credentials, tokenUrl) = _options.GetCredentials(); 
	        
			if (TokenHeaders == null) TokenHeaders = new Dictionary<string, string>();
			
			if (!TokenHeaders.ContainsKey("Host")) TokenHeaders.Add("Host", "fincloud.getbucks.com");
	        if (_options.TokenStore.GetToken() != null)
	        {
		        return _options.TokenStore.GetToken();
	        }
			var result = await tokenUrl
                .AppendPathSegment("tokens")
                .AppendPathSegment(_options.TokenStore.GetToken().RefreshToken)
				.WithHeaders(TokenHeaders)
				.WithBasicAuth(credentials.ClientId, credentials.ClientSecret)
                .PostJsonAsync(new {context = _options.Context}).ReceiveJson<BearerToken>();
	        _options.TokenStore.SetToken(result);
            return result;
        }

        public async Task<BearerToken> GetAuthToken(string email, string password)
        {
	        if (!_options.HasAuthentication)
	        {
		        throw new Exception("Cannot get token. No authentication details specified.");
	        }

	        email.Defend(nameof(email))
		         .NotNullOrEmpty()
		        .Throw();

	        password.Defend(nameof(password))
		        .NotNullOrEmpty()
		        .Throw();
	        
			if (TokenHeaders == null) TokenHeaders = new Dictionary<string, string>();
	        
	        if (_options.TokenStore.GetToken() != null)
	        {
		        return _options.TokenStore.GetToken();
	        }
	        var (credentials, tokenUrl) = _options.GetCredentials(); 
	        
			var accountModel = new UserAuthenticationRequest
            {
                Context = _options.Context,
                Email = email,
                Password = password,
                MobileNumber = ""
            };
            var result = await tokenUrl
                .AppendPathSegment("tokens")
                .WithBasicAuth(credentials.ClientId, credentials.ClientSecret)
                .PostJsonAsync(accountModel)
                .ReceiveJson<BearerToken>();
	        _options.TokenStore.SetToken(result);
            return result;
        } 
	    
	    public async Task<BearerToken> GetClientCredentialsAuthToken(string [] scopes)
        {
	        if (!_options.HasAuthentication)
	        {
		        throw new Exception("Cannot get token. No authentication details specified.");
	        }

	        if (!scopes?.Any() ?? false)
	        {
		        throw new Exception("No scopes specified");
	        }
	    
	        
			if (TokenHeaders == null) TokenHeaders = new Dictionary<string, string>();
	        
	        if (_options.TokenStore.GetToken() != null)
	        {
		        return _options.TokenStore.GetToken();
	        }
	        var (credentials, tokenUrl) = _options.GetCredentials(); 
	        
			
            var result = await tokenUrl

                .WithBasicAuth(credentials.ClientId, credentials.ClientSecret)
                .PostMultipartAsync(content =>
	            {
		            content.AddString("client_id", credentials.ClientId);
		            content.AddString("client_secret", credentials.ClientSecret);
		            content.AddString("grant_type", "client_credentials");
		            content.AddString("requested_scopes", string.Join(" ", scopes));
		            
	            })
                .ReceiveJson<BearerToken>();
	        _options.TokenStore.SetToken(result);
            return result;
        }

        public IFlurlRequest GetRequest()
        {
	        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
	        {
		        throw new ArgumentNullException(nameof(_options.BaseUrl));
	        }
	        
            var token = _options.TokenStore.GetToken();
            if (token == null)
            {
	            return _options.BaseUrl.WithHeaders(_options.Headers);
            }

            return _options.BaseUrl
	            .WithOAuthBearerToken(token.AccessToken)
	            .WithHeaders(_options.Headers);
        }
    }
}