using System;
using System.Threading.Tasks;
using Flurl.Http;
using MyBucks.Core.ApiGateway.ApiClient;

namespace Tests
{
    public class TestApiClient : BaseApiClient
    {
        public override string EndpointKey => "customer";
        
        public async Task<object> GetCustomer(Int32 customerId)
        {
            var result = await Client.GetRequest().AppendPathSegment("/v1.0/customers").AppendPathSegment(customerId)
                .GetJsonAsync<object>();
            return result;
        }
    }
}