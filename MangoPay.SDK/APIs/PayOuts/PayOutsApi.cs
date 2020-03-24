using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.PayOuts
{
    public class PayOutsApi : BaseApi, IPayOutsApi
    {
        public PayOutsApi(
           MangoPayApiConfiguration config,
           ILogger<PayOutsApi> logger,
           IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<PayOutBankWireDTO> CreateBankWire(PayOutBankWirePostDTO payOut)
        {
            var targetUrl = $"{_baseUrl}/payouts/bankwire";
            return await CreateEntity<PayOutBankWireDTO, PayOutBankWirePostDTO>(targetUrl, payOut);
        }

        public async Task<PayOutBankWireDTO> CreateBankWire(string idempotencyKey, PayOutBankWirePostDTO payOut)
        {
            var targetUrl = $"{_baseUrl}/payouts/bankwire";
            return await CreateEntity<PayOutBankWireDTO, PayOutBankWirePostDTO>(targetUrl, payOut, idempotencyKey);
        }

        public async Task<PayOutDTO> Get(string payOutId)
        {
            var targetUrl = $"{_baseUrl}/payouts/{payOutId}";
            return await GetEntity<PayOutDTO>(targetUrl);
        }

        public async Task<PayOutBankWireDTO> GetBankWire(string payOutId)
        {
            var targetUrl = $"{_baseUrl}/payouts/{payOutId}";
            return await GetEntity<PayOutBankWireDTO>(targetUrl);
        }
    }
}
