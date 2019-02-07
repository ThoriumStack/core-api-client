namespace MyBucks.Core.ApiGateway.ApiClient
{
    // TODO: To get a client more easily
    public abstract class BaseApiClient
    {
        protected MyBucksApiClient Client;

        public void Initialize(MyBucksApiClient client)
        {
            Client = client;
        }
        
        public abstract string EndpointKey { get; } 
        
    }
}