namespace Thorium.Core.ApiGateway.ApiClient
{
    public abstract class BaseApiClient
    {
        // likely exposing too much here, but accessing the client headers etc... might be important after config stage. Oh well?
        public ApiClient Client;

        public void Initialize(ApiClient client)
        {
            Client = client;
        }
        
        
        
        public abstract string EndpointKey { get; } 
        
    }
}