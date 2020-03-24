using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.Transport;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Reports
{
    public class ReportsApi : BaseApi, IReportsApi
    {
        public ReportsApi(
        MangoPayApiConfiguration config,
        ILogger<ReportsApi> logger,
        IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<ReportRequestDTO> Create(ReportRequestPostDTO reportRequest)
        {
            if (!reportRequest.ReportType.HasValue) reportRequest.ReportType = ReportType.TRANSACTIONS;

            return await Create(null, reportRequest);
        }

        public async Task<ReportRequestDTO> Create(string idempotencyKey, ReportRequestPostDTO reportRequest)
        {
            if (!reportRequest.ReportType.HasValue) reportRequest.ReportType = ReportType.TRANSACTIONS;

            ReportRequestTransportPostDTO reportRequestTransport = ReportRequestTransportPostDTO.CreateFromBusinessObject(reportRequest);

            var targetUrl = $"{_baseUrl}/reports/{reportRequestTransport.ReportType.ToString().ToLower()}";
            
            var result = await CreateEntity<ReportRequestTransportDTO, ReportRequestTransportPostDTO>(targetUrl, reportRequestTransport, idempotencyKey);

            return result.GetBusinessObject();
        }

        public async Task<ReportRequestDTO> Get(string reportId)
        {
            var targetUrl = $"{_baseUrl}/reports/{reportId}";
            return await GetEntity<ReportRequestDTO>(targetUrl);
        }

        public async Task<ListPaginated<ReportRequestDTO>> GetAll(Pagination pagination, FilterReportsList filters = null, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/reports/";
            if (filters == null) filters = new FilterReportsList();
            return await GetList<ReportRequestDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<ListPaginated<ReportRequestDTO>> GetAll()
        {
            var targetUrl = $"{_baseUrl}/reports/";
            return await GetList<ReportRequestDTO>(targetUrl, null, null);
        }
    }
}
