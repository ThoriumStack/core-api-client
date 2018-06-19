using System;
using System.Threading.Tasks;
using MyBucks.Core.ApiGateway.ApiClient.Models;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            
            var client = new MyBucks.Core.ApiGateway.ApiClient.MyBucksApiClient();
            client.SetTokenBaseUrl("https://testapi.getsure.info");
            client.WithContext("mybucks.getsure.za");
            client.WithAuthentication(new TokenAuthenticationCredentials
            {
                AppName = "getsure_admin",
                ClientId = "getsure_admin",
                ClientSecret = "NRt9DVNkTnA6"
            });

            var token = await client.GetAuthToken("test@getsure.com", "password1");
            return;
        }
    }
}