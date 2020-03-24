using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Hooks
{
    public class HooksApi : BaseApi, IHooksApi
    {
        public HooksApi(
          MangoPayApiConfiguration config,
          ILogger<HooksApi> logger,
          IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<HookDTO> Create(HookPostDTO hook)
        {
            var targetUrl = $"{_baseUrl}/hooks";
            return await CreateEntity<HookDTO, HookPostDTO>(targetUrl, hook);
        }

        public async Task<HookDTO> Create(string idempotencyKey, HookPostDTO hook)
        {
            var targetUrl = $"{_baseUrl}/hooks";
            return await CreateEntity<HookDTO, HookPostDTO>(targetUrl, hook, idempotencyKey);
        }

        public async Task<HookDTO> Get(string hookId)
        {
            var targetUrl = $"{_baseUrl}/hooks/{hookId}";
            return await GetEntity<HookDTO>(targetUrl);
        }

        public async Task<ListPaginated<HookDTO>> GetAll(Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/hooks";
            return await GetList<HookDTO>(targetUrl, pagination, sort);
        }

        public async Task<ListPaginated<HookDTO>> GetAll()
        {
            return await GetAll(null);
        }

        public async Task<HookDTO> Update(HookPutDTO hook, string hookId)
        {
            var targetUrl = $"{_baseUrl}/hooks/{hookId}";
            return await UpdateEntity<HookDTO, HookPutDTO>(targetUrl, hook);
        }
    }
}
