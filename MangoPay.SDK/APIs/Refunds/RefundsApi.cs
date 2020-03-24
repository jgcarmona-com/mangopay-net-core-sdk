using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Refunds
{
    public class RefundsApi: BaseApi, IRefundsApi
    {
        public RefundsApi(
          MangoPayApiConfiguration config,
          ILogger<RefundsApi> logger,
          IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public Task<RefundDTO> Get(string refundId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListPaginated<RefundDTO>> GetRefundsForPayIn(string payInId, Pagination pagination, FilterRefunds filters, Sort sort = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListPaginated<RefundDTO>> GetRefundsForPayOut(string payOutId, Pagination pagination, FilterRefunds filters, Sort sort = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListPaginated<RefundDTO>> GetRefundsForRepudiation(string repudiationId, Pagination pagination, FilterRefunds filters, Sort sort = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListPaginated<RefundDTO>> GetRefundsForTransfer(string transferId, Pagination pagination, FilterRefunds filters, Sort sort = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
