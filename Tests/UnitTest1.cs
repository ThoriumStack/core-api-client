using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBucks.Core.ApiGateway.ApiClient;
using MyBucks.Core.ApiGateway.ApiClient.Models;
using SimpleInjector;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
//            var client = new MyBucks.Core.ApiGateway.ApiClient.MyBucksApiClient()
//                .Configure("https://testapi.getsure.info", "mybucks.getsure.za",
//                    options =>
//                    {
//                        options.EnableAuthentication("https://testapi.getsure.info", "getsure_admin", "NRt9DVNkTnA6",
//                            "getsure_admin");
//                        options.TokenStore = new DefaultTokenStore();
//                    });
//
//            var token = await client.GetAuthToken("test@getsure.com", "password1");
//            return;
        }

        [Fact]
        public async Task TestGetToken()
        {
            var client = new MyBucks.Core.ApiGateway.ApiClient.MyBucksApiClient()
                .Configure("http://localhost:5000/v1.0/insurance/credit-life-policies", "mybucks.getsure.za", "",0, options =>
                {
                    options.EnableAuthentication("http://localhost:5000/connect/token", "fincloud_credit_life", "634AD939-36A9-4CD4-A58C-5E8481D7A466", "fincloud");
                });
                    

            var token = await client.GetClientCredentialsAuthToken(new[] {"credit_life"});
            return;
        }

        [Fact]
        public async Task TestFactory()
        {
            var container = new Container();
            
            container.Register(() => new List<ServiceEndpointSettings>
            {
                new ServiceEndpointSettings
                {
                    Name = "customer",
                    Url = "http://localhost:50063"
                },
                new ServiceEndpointSettings
                {
                    Name = "notcustomer",
                    Url = "http://localhost:50062"
                }
            });
            
            container.Register<ApiClientFactory>();
            
            container.Register<ServiceSimulator>();

            var svc = container.GetInstance<ServiceSimulator>();
            var customer = await svc.GetCustomer();
            
            Assert.NotNull(customer);
            
        }
        
    }
}