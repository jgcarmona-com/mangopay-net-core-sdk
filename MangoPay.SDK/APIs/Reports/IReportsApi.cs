using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Reports
{
    public interface IReportsApi
    {   /// <summary>Creates new report request.</summary>
        /// <param name="hook">Report request instance to be created.</param>
        /// <returns>Report request instance returned from API.</returns>
        Task<ReportRequestDTO> Create(ReportRequestPostDTO reportRequest);


        /// <summary>Creates new report request.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="hook">Report request instance to be created.</param>
        /// <returns>Report request instance returned from API.</returns>
        Task<ReportRequestDTO> Create(string idempotencyKey, ReportRequestPostDTO reportRequest);

        /// <summary>Gets report request.</summary>
        /// <param name="hookId">Report request identifier.</param>
        /// <returns>Report request instance returned from API.</returns>
        Task<ReportRequestDTO> Get(string reportId);


        /// <summary>Gets all report requests.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of ReportRequest instances returned from API.</returns>
        Task<ListPaginated<ReportRequestDTO>> GetAll(Pagination pagination, FilterReportsList filters = null, Sort sort = null);

        /// <summary>Gets all report requests.</summary>
        /// <returns>List of ReportRequest instances returned from API.</returns>
		Task<ListPaginated<ReportRequestDTO>> GetAll();
    }
}
