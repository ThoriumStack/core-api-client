using Thorium.Core.ApiGateway.ApiClient.Models;

namespace Thorium.Core.ApiGateway.ApiClient
{
    public class DefaultTokenStore : ITokenStore
    {
        private BearerToken _token;

        public BearerToken GetToken()
        {
            return _token;
        }

        public void SetToken(BearerToken token)
        {
            _token = token;
        }
    }
}