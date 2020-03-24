using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.CardPreAuthorizations
{
    public class CardPreAuthorizationsApi : BaseApi, ICardPreAuthorizationsApi
    {
        public CardPreAuthorizationsApi(
            MangoPayApiConfiguration config,
            ILogger<CardPreAuthorizationsApi> logger,
            IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<CardPreAuthorizationDTO> Create(CardPreAuthorizationPostDTO cardPreAuthorization)
        {
            return await Create(null, cardPreAuthorization);
        }

        public async Task<CardPreAuthorizationDTO> Create(string idempotencyKey, CardPreAuthorizationPostDTO cardPreAuthorization)
        {
            var targetUrl = $"{_baseUrl}/preauthorizations/card/direct";
            return await CreateEntity<CardPreAuthorizationDTO, CardPreAuthorizationPostDTO>(targetUrl, cardPreAuthorization, idempotencyKey);
        }

        public async Task<CardPreAuthorizationDTO> Get(string cardPreAuthorizationId)
        {
            var targetUrl = $"{_baseUrl}/preauthorizations/{cardPreAuthorizationId}";
            return await GetEntity<CardPreAuthorizationDTO>(targetUrl);
        }

        public async Task<ListPaginated<CardPreAuthorizationDTO>> GetPreAuthorizationsForCard(string cardId, Pagination pagination, FilterPreAuthorizations filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/cards/{cardId}/preauthorizations";
            return await GetList<CardPreAuthorizationDTO>(targetUrl, pagination, sort);
        }

        public async Task<ListPaginated<CardPreAuthorizationDTO>> GetPreAuthorizationsForUser(string userId, Pagination pagination, FilterPreAuthorizations filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/preauthorizations";
            return await GetList<CardPreAuthorizationDTO>(targetUrl, pagination, sort);
        }

        public async Task<CardPreAuthorizationDTO> Update(CardPreAuthorizationPutDTO cardPreAuthorization, string cardPreAuthorizationId)
        {
            var targetUrl = $"{_baseUrl}/preauthorizations/{cardPreAuthorizationId}";
            return await UpdateEntity<CardPreAuthorizationDTO, CardPreAuthorizationPutDTO>(targetUrl, cardPreAuthorization);
        }
    }
}
