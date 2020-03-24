using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Idempotency
{
    public class IdempotencyApi : BaseApi, IIdempotencyApi
    {
        public IdempotencyApi(
              MangoPayApiConfiguration config,
              ILogger<IdempotencyApi> logger,
              IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }
    }
}
