using System;
using System.Collections.Generic;

namespace MyBucks.Core.ApiGateway.ApiClient.Models
{
    public class MyBucksApiOptions
    {
        internal string Context { get; set; }
        private string _tokenBaseUrl;
        public ITokenStore TokenStore { get; set; }
        internal Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        
        private TokenAuthenticationCredentials _tokenAuthenticationCredentials;

        public void AddHeader(string key, string value)
        {
            Headers.Add(key, value);
        }
        
        internal void WithContext(string context)
        {
            Context = context;
            if (!Headers.ContainsKey("MyBucks-Context"))
            {
                Headers.Add("MyBucks-Context", context);
            }
        }

        public void EnableAuthentication(string tokenBaseUrl, string clientId, string clientSecret, string appName)
        {
            _tokenBaseUrl = tokenBaseUrl;

            CheckForBlank(tokenBaseUrl, "Token base Url");

            if (Uri.TryCreate(tokenBaseUrl, UriKind.Absolute, out Uri _))
            {
                throw new Exception("Token base url invalid.");
            }

            tokenBaseUrl
                .Defend(nameof(tokenBaseUrl))
                .ValidUri()
                .Custom(s => s.Length > 0, "{0} Length must be more than 0");

            clientId
                .Defend(nameof(clientId))
                .NotNullOrEmpty();
            
            clientSecret
                .Defend(nameof(clientSecret))
                .NotNullOrEmpty();
            
            appName
                .Defend(nameof(appName))
                .NotNullOrEmpty();
            
            _tokenAuthenticationCredentials = new TokenAuthenticationCredentials
            {
                AppName = appName,
                ClientId = clientId,
                ClientSecret = clientSecret
            };
        }

        private void CheckForBlank(string input, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("{parameterName} cannot be null or blank.");
            }
        }

        internal (TokenAuthenticationCredentials credentials, string tokenBaseUrl) GetCredentials()
        {
            return (_tokenAuthenticationCredentials, _tokenBaseUrl);
        }

        internal bool HasAuthentication =>
            !string.IsNullOrWhiteSpace(_tokenBaseUrl) && _tokenAuthenticationCredentials != null;

        public string BaseUrl { get; internal set; }
    }
}