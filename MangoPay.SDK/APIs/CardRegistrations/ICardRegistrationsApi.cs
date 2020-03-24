using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.CardRegistrations
{
    public interface ICardRegistrationsApi
    {
        /// <summary>Creates new card registration.</summary>
        /// <param name="cardRegistration">Card registration object to create.</param>
        /// <returns>Card registration object returned from API.</returns>
        Task<CardRegistrationDTO> Create(CardRegistrationPostDTO cardRegistration);

        /// <summary>Creates new card registration.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="cardRegistration">Card registration object to create.</param>
        /// <returns>Card registration object returned from API.</returns>
        Task<CardRegistrationDTO> Create(string idempotencyKey, CardRegistrationPostDTO cardRegistration);


        /// <summary>Gets card registration.</summary>
        /// <param name="cardRegistrationId">Card registration identifier.</param>
        /// <returns>Card registration instance returned from API.</returns>
        Task<CardRegistrationDTO> Get(string cardRegistrationId);

        /// <summary>Updates card registration.</summary>
        /// <param name="cardRegistration">Card registration instance to be updated.</param>
        /// <param name="cardRegistrationId">Card registration identifier.</param>
        /// <returns>Card registration object returned from API.</returns>
        Task<CardRegistrationDTO> Update(CardRegistrationPutDTO cardRegistration, string cardRegistrationId);
    }
}

