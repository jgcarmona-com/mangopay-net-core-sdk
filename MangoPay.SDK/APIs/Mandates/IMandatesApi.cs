using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Mandates
{
    public interface IMandatesApi
    {
        /// <summary>Creates new mandate.</summary>
        /// <param name="mandate">Mandate instance to be created.</param>
        /// <returns>Mandate instance returned from API.</returns>
        Task<MandateDTO> Create(MandatePostDTO mandate);

        /// <summary>Creates new mandate.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="mandate">Mandate instance to be created.</param>
        /// <returns>Mandate instance returned from API.</returns>
        Task<MandateDTO> Create(string idempotencyKey, MandatePostDTO mandate);

        /// <summary>Gets mandate.</summary>
        /// <param name="mandateId">Mandate identifier.</param>
        /// <returns>Mandate instance returned from API.</returns>
        Task<MandateDTO> Get(string mandateId);

        /// <summary>Gets all mandates.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filters">Filters.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of Mandate instances returned from API.</returns>
        Task<ListPaginated<MandateDTO>> GetAll(Pagination pagination, FilterMandates filters, Sort sort = null);

        /// <summary>Gets all mandates.</summary>
        /// <returns>List of Mandate instances returned from API.</returns>
        Task<ListPaginated<MandateDTO>> GetAll();

        /// <summary>Gets mandates for user.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filters">Filters.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of Mandate instances returned from API.</returns>
        Task<ListPaginated<MandateDTO>> GetForUser(string userId, Pagination pagination, FilterMandates filters, Sort sort = null);

        /// <summary>Gets mandates for bank account.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="bankAccountId">Bank account identifier.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filters">Filters.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of Mandate instances returned from API.</returns>
        Task<ListPaginated<MandateDTO>> GetForBankAccount(string userId, string bankAccountId, Pagination pagination, FilterMandates filters, Sort sort = null);

        /// <summary>Cancels mandate.</summary>
        /// <param name="mandateId">Mandate identifier.</param>
        /// <returns>Mandate instance returned from API.</returns>
        Task<MandateDTO> Cancel(string mandateId);

        /// <summary>Lists transactions for a mandate</summary>
        /// <param name="mandateId">Id of the mandate to get transactions</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of transactions for a mandate</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactionsForMandate(string mandateId, Pagination pagination, FilterTransactions filters, Sort sort = null);
    }
}