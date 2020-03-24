using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Cards
{
    public class CardsApi : BaseApi, ICardsApi
    {
        public CardsApi(
            MangoPayApiConfiguration config,
            ILogger<CardsApi> logger,
            IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<CardDTO> Get(string cardId)
        {
            var targetUrl = $"{_baseUrl}/cards/{cardId}";
            return await GetEntity<CardDTO>(targetUrl);
        }

        public async Task<ListPaginated<CardDTO>> GetCardsByFingerprint(string fingerprint)
        {
            var targetUrl = $"{_baseUrl}/cards/fingerprints/{fingerprint}";
            return await  GetCardsByFingerprint(fingerprint, null, null);
        }

        public async Task<ListPaginated<CardDTO>> GetCardsByFingerprint(string fingerprint, Pagination pagination, Sort sort)
        {
            var targetUrl = $"{_baseUrl}/cards/fingerprints/{fingerprint}";
            return await GetCardsByFingerprint(fingerprint, pagination, sort);
        }

        public async  Task<ListPaginated<TransactionDTO>> GetTransactionsForCard(string cardId, Pagination pagination, FilterTransactions filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/cards/{cardId}/transactions";
            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<CardDTO> Update(CardPutDTO card, string cardId)
        {
            var targetUrl = $"{_baseUrl}/cards/{cardId}";
            return await UpdateEntity<CardDTO, CardPutDTO>(targetUrl, card);
        }
    }
}
