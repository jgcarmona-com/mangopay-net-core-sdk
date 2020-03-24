using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.CardRegistrations
{
    public class CardRegistrationsApi : BaseApi, ICardRegistrationsApi
    {
        public CardRegistrationsApi(
           MangoPayApiConfiguration config,
           ILogger<CardRegistrationsApi> logger,
           IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<CardRegistrationDTO> Create(CardRegistrationPostDTO cardRegistration)
        {
            var targetUrl = $"{_baseUrl}/cardregistrations";
            return await CreateEntity<CardRegistrationDTO, CardRegistrationPostDTO>(targetUrl, cardRegistration);
        }

        public async Task<CardRegistrationDTO> Create(string idempotencyKey, CardRegistrationPostDTO cardRegistration)
        {
            var targetUrl = $"{_baseUrl}/cardregistrations";
            return await CreateEntity<CardRegistrationDTO, CardRegistrationPostDTO>(targetUrl, cardRegistration, idempotencyKey);
          
        }

        public async Task<CardRegistrationDTO> Get(string cardRegistrationId)
        {
            var targetUrl = $"{_baseUrl}/cardregistrations/{cardRegistrationId}";
            return await GetEntity<CardRegistrationDTO>(targetUrl);
        }

        public async Task<CardRegistrationDTO> Update(CardRegistrationPutDTO cardRegistration, string cardRegistrationId)
        {
            var targetUrl = $"{_baseUrl}/cardregistrations/{cardRegistrationId}";
            return await UpdateEntity<CardRegistrationDTO, CardRegistrationPutDTO>(targetUrl, cardRegistration);
        }
    }
}
