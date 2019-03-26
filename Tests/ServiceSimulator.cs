using System.Threading.Tasks;
using Thorium.Core.ApiGateway.ApiClient;

namespace Tests
{
    public class ServiceSimulator
    {
        private readonly ApiClientFactory _factory;

        public ServiceSimulator(ApiClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<object> GetCustomer()
        {
            var client = _factory.GetClient<TestApiClient>("test", "1", 0);
            var customer = await client.GetCustomer(1);
            return customer;


        }
    }
}