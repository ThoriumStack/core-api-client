using System;
using System.Collections.Generic;
using System.Linq;
using Thorium.Core.ApiGateway.ApiClient.Models;

namespace Thorium.Core.ApiGateway.ApiClient
{
    public class ApiClientFactory
    {
        private readonly List<ServiceEndpointSettings> _settings;

        public ApiClientFactory(List<ServiceEndpointSettings> settings)
        {
            _settings = settings;
        }
        
        public TClientType GetClient<TClientType>(string context, string userId, int timezoneOffsetInMinutes, Action<ApiOptions> config = null) where TClientType : BaseApiClient, new()
        {
            
            var inst =  new TClientType();
            var key = _settings.FirstOrDefault(c=>c.Name == inst.EndpointKey)  ;
            if (key == null)
            {
                throw new Exception($"Unable to find endpoint key : {inst.EndpointKey}");
            }
            var mbApi = new ApiClient()
                .Configure(key.Url, context, userId,timezoneOffsetInMinutes,  config);
            inst.Initialize(mbApi);

            return inst;
        }
    }
}