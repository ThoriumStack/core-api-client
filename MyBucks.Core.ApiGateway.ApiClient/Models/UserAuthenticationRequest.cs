namespace MyBucks.Core.ApiGateway.ApiClient.Models
{
    public class UserAuthenticationRequest
    {
        public string Context { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }
}