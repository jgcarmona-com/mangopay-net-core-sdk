using System.Threading.Tasks;
using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.APIs.Kyc
{
    public class KycApi : BaseApi, IKycApi
    {
        public KycApi(
            MangoPayApiConfiguration config,
            ILogger<KycApi> logger,
            IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<KycDocumentDTO> Get(string kycDocumentId)
        {
            var targetUrl = $"{_baseUrl}/KYC/documents/{kycDocumentId}";
            return await GetEntity<KycDocumentDTO>(targetUrl);
        }

        public async Task<ListPaginated<DocumentConsultationDTO>> GetDocumentConsultations(string kycDocumentId)
        {
            var targetUrl = $"{_baseUrl}//KYC/documents/{kycDocumentId}/consult";
            return await GetList<DocumentConsultationDTO>(targetUrl, null);
        }

        public async Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(Pagination pagination, FilterKycDocuments filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/KYC/documents";
            return await GetList<KycDocumentDTO>(targetUrl, pagination, sort);
        }
    }
}
