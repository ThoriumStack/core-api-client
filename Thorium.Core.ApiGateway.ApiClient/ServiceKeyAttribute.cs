namespace Thorium.Core.ApiGateway.ApiClient
{
    public class ServiceKeyAttribute : System.Attribute
    {
        public string Key { get; }

        public ServiceKeyAttribute(string key)
        {
            Key = key;
        }
    }
}