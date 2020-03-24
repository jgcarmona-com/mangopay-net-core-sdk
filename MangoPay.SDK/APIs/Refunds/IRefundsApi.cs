using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Refunds
{
    public interface IRefundsApi
    {
        /// <summary>Gets refund.</summary>
        /// <param name="refundId">Refund identifier.</param>
        /// <returns>Refund entity instance returned from API.</returns>
        Task<RefundDTO> Get(string refundId);

        /// <summary>Lists refunds for a payout</summary>
        /// <param name="payOutId">Id of the payout to get refunds for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of refunds for a payout</returns>
        Task<ListPaginated<RefundDTO>> GetRefundsForPayOut(string payOutId, Pagination pagination, FilterRefunds filters, Sort sort = null);

        /// <summary>Lists refunds for a payin</summary>
        /// <param name="payInId">Id of the payin to get refunds for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of refunds for a payin</returns>
        Task<ListPaginated<RefundDTO>> GetRefundsForPayIn(string payInId, Pagination pagination, FilterRefunds filters, Sort sort = null);

        /// <summary>Lists refunds for a transfer</summary>
        /// <param name="transferId">Id of the transfer to get refunds for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of refunds for a transfer</returns>
        Task<ListPaginated<RefundDTO>> GetRefundsForTransfer(string transferId, Pagination pagination, FilterRefunds filters, Sort sort = null);

        /// <summary>Lists refunds for a repudiation</summary>
        /// <param name="repudiationId">Id of the repudiation to get refunds for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of refunds for a repudiation</returns>
        Task<ListPaginated<RefundDTO>> GetRefundsForRepudiation(string repudiationId, Pagination pagination, FilterRefunds filters, Sort sort = null);
    }
}
