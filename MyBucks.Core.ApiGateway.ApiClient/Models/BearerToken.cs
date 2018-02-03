using Newtonsoft.Json;

namespace MyBucks.Core.ApiGateway.ApiClient.Models
{
    public class BearerToken
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}