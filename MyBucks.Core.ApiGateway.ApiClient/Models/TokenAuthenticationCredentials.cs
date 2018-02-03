namespace MyBucks.Core.ApiGateway.ApiClient.Models
{
    public class TokenAuthenticationCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppName { get; set; }
    }
}