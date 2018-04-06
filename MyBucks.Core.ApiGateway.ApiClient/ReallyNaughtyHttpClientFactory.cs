using System;
using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    
    public class ReallyNaughtyHttpClientFactory : DefaultHttpClientFactory
    {
        // override to customize how HttpClient is created/configured
        public override HttpClient CreateHttpClient(HttpMessageHandler handler)
        {
            
            var newHandler = new HttpClientHandler();
          

                newHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                newHandler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) => { return true; };
            

            var client = new HttpClient(newHandler);
            return client;
        }

        // override to customize how HttpMessageHandler is created/configured
        //public override HttpMessageHandler CreateMessageHandler();
    }
}