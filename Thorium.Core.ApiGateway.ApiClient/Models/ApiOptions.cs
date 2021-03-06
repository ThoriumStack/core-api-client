﻿using System;
using System.Collections.Generic;
using Thorium.FluentDefense;

namespace Thorium.Core.ApiGateway.ApiClient.Models
{
    public class ApiOptions
    {
        internal string Context { get; set; }
        internal string UserId { get; set; }
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
            if (!Headers.ContainsKey("Thorium-Context"))
            {
                Headers.Add("Thorium-Context", context);
            }
        }

        internal void WithUserId(string userId)
        {
            UserId = userId;
            if (!Headers.ContainsKey("Thorium-UserId"))
            {
                Headers.Add("Thorium-UserId", userId);
            }
        }
        
        public void WithTimeZoneOffset(int timeZoneOffSetInMinutes)
        {
            if (!Headers.ContainsKey("Thorium-TimeZone"))
            {
                Headers.Add("Thorium-TimeZone", timeZoneOffSetInMinutes.ToString());
            }
        }

        public void EnableAuthentication(string tokenBaseUrl, string clientId, string clientSecret, string appName)
        {
            _tokenBaseUrl = tokenBaseUrl;

            // create global validator

            tokenBaseUrl
                .Defend(nameof(tokenBaseUrl))
                .ValidUri()
                .Throw();

            clientId
                .Defend(nameof(clientId))
                .NotNullOrEmpty()
                .Throw();

            clientSecret
                .Defend(nameof(clientSecret))
                .NotNullOrEmpty()
                .Throw();

            appName
                .Defend(nameof(appName))
                .NotNullOrEmpty()
                .Throw();

            _tokenAuthenticationCredentials = new TokenAuthenticationCredentials
            {
                AppName = appName,
                ClientId = clientId,
                ClientSecret = clientSecret
            };
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