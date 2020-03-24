using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Kyc
{
    public interface IKycApi
    {
        /// <summary>Gets the list of all the uploaded documents for all users.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of all users' uploaded documents.</returns>
        Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(Pagination pagination, FilterKycDocuments filter, Sort sort = null);

        /// <summary>Gets KYC document.</summary>
        /// <param name="kycDocumentId">KYC document identifier.</param>
        /// <returns>KYC document instance returned from API.</returns>
        Task<KycDocumentDTO> Get(string kycDocumentId);

        /// <summary>
        /// Get consultation for all KYC documents or a Dispute document 
        /// </summary>
        /// <param name="kycDocumentId">KYC document identifier.</param>
        /// <returns>Document consultation list</returns>
        Task<ListPaginated<DocumentConsultationDTO>> GetDocumentConsultations(string kycDocumentId);
    }
}
