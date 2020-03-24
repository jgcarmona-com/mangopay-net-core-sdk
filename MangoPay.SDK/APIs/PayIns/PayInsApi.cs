using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.PayIns
{
    public class PayInsApi : BaseApi, IPayInsApi
    {
        public PayInsApi(
            MangoPayApiConfiguration config,
            ILogger<PayInsApi> logger,
            IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<PayInBankWireDirectDTO> CreateBankWireDirect(PayInBankWireDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/bankwire/direct";
            return await CreateEntity<PayInBankWireDirectDTO, PayInBankWireDirectPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInBankWireDirectDTO> CreateBankWireDirect(string idempotencyKey, PayInBankWireDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/bankwire/direct";
            return await CreateEntity<PayInBankWireDirectDTO, PayInBankWireDirectPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInCardDirectDTO> CreateCardDirect(PayInCardDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/card/direct";
            return await CreateEntity<PayInCardDirectDTO, PayInCardDirectPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInCardDirectDTO> CreateCardDirect(string idempotencyKey, PayInCardDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/card/direct";
            return await CreateEntity<PayInCardDirectDTO, PayInCardDirectPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInCardWebDTO> CreateCardWeb(PayInCardWebPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/card/web";
            return await CreateEntity<PayInCardWebDTO, PayInCardWebPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInCardWebDTO> CreateCardWeb(string idempotencyKey, PayInCardWebPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/card/web";
            return await CreateEntity<PayInCardWebDTO, PayInCardWebPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInDirectDebitDTO> CreateDirectDebit(PayInDirectDebitPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/directdebit/web";
            return await CreateEntity<PayInDirectDebitDTO, PayInDirectDebitPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInDirectDebitDTO> CreateDirectDebit(string idempotencyKey, PayInDirectDebitPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/directdebit/web";
            return await CreateEntity<PayInDirectDebitDTO, PayInDirectDebitPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInMandateDirectDTO> CreateMandateDirectDebit(PayInMandateDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/directdebit/direct";
            return await CreateEntity<PayInMandateDirectDTO, PayInMandateDirectPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInMandateDirectDTO> CreateMandateDirectDebit(string idempotencyKey, PayInMandateDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/directdebit/direct";
            return await CreateEntity<PayInMandateDirectDTO, PayInMandateDirectPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInPayPalDTO> CreatePayPal(PayInPayPalPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/paypal/web";
            return await CreateEntity<PayInPayPalDTO, PayInPayPalPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInPayPalDTO> CreatePayPal(string idempotencyKey, PayInPayPalPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/paypal/web";
            return await CreateEntity<PayInPayPalDTO, PayInPayPalPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<PayInPreauthorizedDirectDTO> CreatePreauthorizedDirect(PayInPreauthorizedDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/preauthorized/direct";
            return await CreateEntity<PayInPreauthorizedDirectDTO, PayInPreauthorizedDirectPostDTO>(targetUrl, payIn);
        }

        public async Task<PayInPreauthorizedDirectDTO> CreatePreauthorizedDirect(string idempotencyKey, PayInPreauthorizedDirectPostDTO payIn)
        {
            var targetUrl = $"{_baseUrl}/payins/preauthorized/direct";
            return await CreateEntity<PayInPreauthorizedDirectDTO, PayInPreauthorizedDirectPostDTO>(targetUrl, payIn, idempotencyKey);
        }

        public async Task<RefundDTO> CreateRefund(string payInId, RefundPayInPostDTO refund)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}/refunds";
            return await CreateEntity<RefundDTO, RefundPayInPostDTO>(targetUrl, refund);
        }

        public async Task<RefundDTO> CreateRefund(string idempotencyKey, string payInId, RefundPayInPostDTO refund)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}/refunds";
            return await CreateEntity<RefundDTO, RefundPayInPostDTO>(targetUrl, refund, idempotencyKey);
        }

        public async Task<PayInDTO> Get(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInDTO>(targetUrl);
        }

        public async Task<PayInBankWireDirectDTO> GetBankWireDirect(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInBankWireDirectDTO>(targetUrl);
        }

        public async Task<PayInBankWireExternalInstructionDTO> GetBankWireExternalInstruction(string payInId)
        {

            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInBankWireExternalInstructionDTO>(targetUrl);
        }

        public async Task<CardDTO> GetCardDataForCardWeb(string payInId)
        {

            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<CardDTO>(targetUrl);
        }

        public async Task<PayInCardDirectDTO> GetCardDirect(string payInId)
        {

            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInCardDirectDTO>(targetUrl);
        }

        public async Task<PayInCardWebDTO> GetCardWeb(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInCardWebDTO>(targetUrl);
        }

        public async Task<PayInDirectDebitDTO> GetDirectDebit(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInDirectDebitDTO>(targetUrl);
        }

        public async Task<PayInMandateDirectDTO> GetMandateDirectDebit(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInMandateDirectDTO>(targetUrl);
        }

        public async Task<PayInPayPalDTO> GetPayPal(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInPayPalDTO>(targetUrl);
        }

        public async Task<PayInPreauthorizedDirectDTO> GetPreauthorizedDirect(string payInId)
        {
            var targetUrl = $"{_baseUrl}/payins/{payInId}";
            return await GetEntity<PayInPreauthorizedDirectDTO>(targetUrl);
        }
    }
}
