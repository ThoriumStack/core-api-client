using System;
using System.Threading.Tasks;
using MyBucks.Core.ApiGateway.ApiClient;
using MyBucks.Core.ApiGateway.ApiClient.Models;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var client = new MyBucks.Core.ApiGateway.ApiClient.MyBucksApiClient()
                .Configure("https://testapi.getsure.info", "mybucks.getsure.za",
                    options =>
                    {
                        options.EnableAuthentication("https://testapi.getsure.info", "getsure_admin", "NRt9DVNkTnA6",
                            "getsure_admin");
                        options.TokenStore = new DefaultTokenStore();
                    });

            var token = await client.GetAuthToken("test@getsure.com", "password1");
            return;
        }
    }
}