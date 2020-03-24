using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Mandates
{
    class MandatesApi : BaseApi, IMandatesApi
    {
        public MandatesApi(
        MangoPayApiConfiguration config,
        ILogger<MandatesApi> logger,
        IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<MandateDTO> Cancel(string mandateId)
        {
            var targetUrl = $"{_baseUrl}/mandates/{mandateId}";
            return await UpdateEntity<MandateDTO, EntityPutBase>(targetUrl, new EntityPutBase());
        }

        public async Task<MandateDTO> Create(MandatePostDTO mandate)
        {
            var targetUrl = $"{_baseUrl}/mandates/";
            return await CreateEntity<MandateDTO, MandatePostDTO>(targetUrl, mandate);
        }

        public async Task<MandateDTO> Create(string idempotencyKey, MandatePostDTO mandate)
        {
            var targetUrl = $"{_baseUrl}/mandates/";
            return await CreateEntity<MandateDTO, MandatePostDTO>(targetUrl, mandate, idempotencyKey);
        }

        public async Task<MandateDTO> Get(string mandateId)
        {
            var targetUrl = $"{_baseUrl}/mandates/{0}/";
            return await GetEntity<MandateDTO>(targetUrl);
        }

        public async Task<ListPaginated<MandateDTO>> GetAll(Pagination pagination, FilterMandates filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/mandates/";
            if (filters == null) filters = new FilterMandates();
            return await GetList<MandateDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<ListPaginated<MandateDTO>> GetAll()
        {
            return await GetAll(null, null);
        }

        public async Task<ListPaginated<MandateDTO>> GetForBankAccount(string userId, string bankAccountId, Pagination pagination, FilterMandates filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}/mandates/";
            if (filters == null) filters = new FilterMandates();
            return await GetList<MandateDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<ListPaginated<MandateDTO>> GetForUser(string userId, Pagination pagination, FilterMandates filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/mandates/";
            if (filters == null) filters = new FilterMandates();
            return await GetList<MandateDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactionsForMandate(string mandateId, Pagination pagination, FilterTransactions filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/mandates/{mandateId}/transactions";
            if (filters == null) filters = new FilterTransactions();
            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filters.GetValues());
        }
    }
}
