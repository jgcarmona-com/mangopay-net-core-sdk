using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Wallets
{
    public class WalletsApi : BaseApi, IWalletsApi
    {
        public WalletsApi(
          MangoPayApiConfiguration config,
          ILogger<WalletsApi> logger,
          IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<WalletDTO> Create(WalletPostDTO wallet)
        {
            return await Create(null, wallet);
        }

        public async Task<WalletDTO> Create(string idempotencyKey, WalletPostDTO wallet)
        {
            var targetUrl = $"{_baseUrl}/wallets";
            return await CreateEntity<WalletDTO, WalletPostDTO>(targetUrl, wallet, idempotencyKey);
        }

        public async Task<WalletDTO> Get(string walletId)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}";
            return await GetEntity<WalletDTO>(targetUrl);
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId, Pagination pagination, FilterTransactions filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}/transactions";
            if (filter == null) filter = new FilterTransactions();
            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId, Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}/transactions";
            return await GetList<TransactionDTO>(targetUrl, pagination, sort);
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}/transactions";
            return await GetList<TransactionDTO>(targetUrl, null);
        }

        public async Task<WalletDTO> Update(WalletPutDTO wallet, string walletId)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}";
            return await UpdateEntity<WalletDTO, WalletPutDTO>(targetUrl, wallet);
        }
    }
}
