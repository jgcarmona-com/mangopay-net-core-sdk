using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.CardPreAuthorizations
{
    public interface ICardPreAuthorizationsApi
    {
        /// <summary>Creates new pre-authorization object.</summary>
        /// <param name="cardPreAuthorization">PreAuthorization object to be created.</param>
        /// <returns>Card registration instance returned from API.</returns>
        Task<CardPreAuthorizationDTO> Create(CardPreAuthorizationPostDTO cardPreAuthorization);

        /// <summary>Creates new pre-authorization object.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="cardPreAuthorization">PreAuthorization object to be created.</param>
        /// <returns>Card registration instance returned from API.</returns>
        Task<CardPreAuthorizationDTO> Create(string idempotencyKey, CardPreAuthorizationPostDTO cardPreAuthorization);

        /// <summary>Gets pre-authorization object.</summary>
        /// <param name="cardPreAuthorizationId">PreAuthorization identifier.</param>
        /// <returns>Card registration instance returned from API.</returns>
        Task<CardPreAuthorizationDTO> Get(string cardPreAuthorizationId);

        /// <summary>Updates pre-authorization object.</summary>
        /// <param name="cardPreAuthorization">PreAuthorization object to be updated.</param>
        /// <param name="cardPreAuthorizationId">PreAuthorization object identifier.</param>
        /// <returns>Card registration instance returned from API.</returns>
        Task<CardPreAuthorizationDTO> Update(CardPreAuthorizationPutDTO cardPreAuthorization, string cardPreAuthorizationId);

        /// <summary>Lists PreAuthorizations for a user</summary>
        /// <param name="userId">Id of the user to get PreAuthorizations for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of PreAuthorizations for a user</returns>
        Task<ListPaginated<CardPreAuthorizationDTO>> GetPreAuthorizationsForUser(string userId, Pagination pagination, FilterPreAuthorizations filters, Sort sort = null);

        /// <summary>Lists PreAuthorizations for a card</summary>
        /// <param name="cardId">Id of the card to get PreAuthorizations for</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of PreAuthorizations for a card</returns>
        Task<ListPaginated<CardPreAuthorizationDTO>> GetPreAuthorizationsForCard(string cardId, Pagination pagination, FilterPreAuthorizations filters, Sort sort = null);
    }
}
