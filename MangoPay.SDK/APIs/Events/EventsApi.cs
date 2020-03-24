using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Events
{
    public class EventsApi: BaseApi, IEventsApi
    {
        public EventsApi(
          MangoPayApiConfiguration config,
          ILogger<EventsApi> logger,
          IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        /// <summary>Gets events.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filters for events.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of events matching passed filter criteria.</returns>
        public async  Task<ListPaginated<EventDTO>> GetAll(Pagination pagination, FilterEvents filter = null, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}";
            if (filter == null) filter = new FilterEvents();
            return await GetList<EventDTO>(targetUrl, pagination, sort, filter.GetValues());
        }
    }
}
