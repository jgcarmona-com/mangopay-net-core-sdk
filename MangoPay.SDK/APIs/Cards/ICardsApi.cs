using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.PUT;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Cards
{
    public interface ICardsApi
    { /// <summary>Gets card.</summary>
      /// <param name="cardId">Card identifier.</param>
      /// <returns>Card instance returned from API.</returns>
        Task<CardDTO> Get(string cardId);

        /// <summary>Saves card.</summary>
        /// <param name="card">Card instance to be updated.</param>
        /// <param name="cardId">Card identifier.</param>
        /// <returns>Card instance returned from API.</returns>
        Task<CardDTO> Update(CardPutDTO card, string cardId);

        /// <summary>Lists transactions for a card</summary>
        /// <param name="cardId">Id of the card to get transactions</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of transactions for a card</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactionsForCard(string cardId, Pagination pagination, FilterTransactions filters, Sort sort = null);
        
        /// <summary>
        /// Gets a list of cards having the same fingerprint.
        /// </summary>
        /// <param name="fingerprint">The fingerprint hash</param>
        /// <returns>List of Cards corresponding to provided fingerprint</returns>
        Task<ListPaginated<CardDTO>> GetCardsByFingerprint(string fingerprint);

        /// <summary>
        /// Gets a list of cards having the same fingerprint.
        /// </summary>
        /// <param name="fingerprint">The fingerprint hash</param>
        /// <param name="pagination">The pagionation object</param>
        /// <param name="sort">The sort object</param>
        /// <returns></returns>
        Task<ListPaginated<CardDTO>> GetCardsByFingerprint(string fingerprint, Pagination pagination, Sort sort);
    }
}
