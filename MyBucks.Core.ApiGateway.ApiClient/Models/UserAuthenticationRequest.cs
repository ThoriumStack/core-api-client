using Newtonsoft.Json;

namespace MyBucks.Core.ApiGateway.ApiClient.Models
{
    public class UserAuthenticationRequest
    {
        [JsonProperty("context")]
        public string Context { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("mobile_number")]
        public string MobileNumber { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}