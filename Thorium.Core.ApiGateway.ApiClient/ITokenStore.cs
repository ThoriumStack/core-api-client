using Thorium.Core.ApiGateway.ApiClient.Models;

namespace Thorium.Core.ApiGateway.ApiClient
{
    public interface ITokenStore
    {
        BearerToken GetToken();
        void SetToken(BearerToken token);
    }
}