using MyBucks.Core.ApiGateway.ApiClient.Models;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    public interface ITokenStore
    {
        BearerToken GetToken();
        void SetToken(BearerToken token);
    }
}