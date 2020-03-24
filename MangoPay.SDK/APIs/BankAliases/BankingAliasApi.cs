using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.BankAliases
{
    public class BankingAliasApi : BaseApi, IBankingAliasApi
    {
        public BankingAliasApi(
         MangoPayApiConfiguration config,
         ILogger<BankingAliasApi> logger,
         IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<BankingAliasIbanDTO> CreateIban(string walletId, BankingAliasIbanPostDTO bankingAlias)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}/bankingaliases/iban";
            return await CreateEntity<BankingAliasIbanDTO, BankingAliasIbanPostDTO>(targetUrl, bankingAlias);
        }

        public async Task<BankingAliasDTO> Get(string bankingAliasId)
        {
            var targetUrl = $"/bankingaliases/{bankingAliasId}";
            return await GetEntity<BankingAliasDTO>(targetUrl);
        }

        public async Task<ListPaginated<BankingAliasDTO>> GetAll(string walletId, Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/wallets/{walletId}/bankingaliases";
            return await GetList<BankingAliasDTO>(targetUrl, pagination, sort);
        }

        public async Task<BankingAliasIbanDTO> GetIban(string bankingAliasId)
        {
            var targetUrl = $"{_baseUrl}/bankingaliases/{bankingAliasId}";
            return await GetEntity<BankingAliasIbanDTO>(targetUrl);
        }

        public async Task<BankingAliasDTO> Update(BankingAliasPutDTO bankingAlias, string bankingAliasId)
        {
            var targetUrl = $"{_baseUrl}//bankingaliases/{bankingAliasId}";
            return await UpdateEntity<BankingAliasDTO, BankingAliasPutDTO>(targetUrl, bankingAlias);
        }
    }
}
