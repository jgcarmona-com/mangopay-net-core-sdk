using MangoPay.SDK.APIs.Auth.DTOs;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Auth
{
    public interface IAuthApi
    {
        Task<OAuthTokenDTO> GetAuthToken();
    }
}
