using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Transfers
{
    public class TransfersApi : BaseApi, ITransfersApi
    {
        public TransfersApi(
          MangoPayApiConfiguration config,
          ILogger<TransfersApi> logger,
          IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<TransferDTO> Create(TransferPostDTO transfer)
        {
            var targetUrl = $"{_baseUrl}/transfers";
            return await CreateEntity<TransferDTO, TransferPostDTO>(targetUrl, transfer);
        }

        public async Task<TransferDTO> Create(string idempotencyKey, TransferPostDTO transfer)
        {
            var targetUrl = $"{_baseUrl}/transfers";
            return await CreateEntity<TransferDTO, TransferPostDTO>(targetUrl, transfer, idempotencyKey);
        }

        public async Task<RefundDTO> CreateRefund(string transferId, RefundTransferPostDTO refund)
        {
            var targetUrl = $"{_baseUrl}/transfers/{transferId}/refunds";
            return await CreateEntity<RefundDTO, RefundTransferPostDTO>(targetUrl, refund);
        }

        public async Task<RefundDTO> CreateRefund(string idempotencyKey, string transferId, RefundTransferPostDTO refund)
        {
            var targetUrl = $"{_baseUrl}/transfers/{transferId}/refunds";
            return await CreateEntity<RefundDTO, RefundTransferPostDTO>(targetUrl, refund, idempotencyKey);
        }

        public async Task<TransferDTO> Get(string transferId)
        {
            var targetUrl = $"{_baseUrl}/transfers/{transferId}";
            return await GetEntity<TransferDTO>(targetUrl);
        }
    }
}
