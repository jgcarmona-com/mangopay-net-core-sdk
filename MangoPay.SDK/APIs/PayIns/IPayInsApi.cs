using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.PayIns
{
    public interface IPayInsApi
    { /// <summary>Creates new payin bankwire direct.</summary>
      /// <param name="payIn">Object instance to be created.</param>
      /// <returns>Object instance returned from API.</returns>
        Task<PayInBankWireDirectDTO> CreateBankWireDirect(PayInBankWireDirectPostDTO payIn);

        /// <summary>Creates new payin bankwire direct.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInBankWireDirectDTO> CreateBankWireDirect(string idempotencyKey, PayInBankWireDirectPostDTO payIn);

        /// <summary>Creates new payin card direct.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInCardDirectDTO> CreateCardDirect(PayInCardDirectPostDTO payIn);

        /// <summary>Creates new payin card direct.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInCardDirectDTO> CreateCardDirect(string idempotencyKey, PayInCardDirectPostDTO payIn);

        /// <summary>Creates new payin card web.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInCardWebDTO> CreateCardWeb(PayInCardWebPostDTO payIn);

        /// <summary>Creates new payin card web.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInCardWebDTO> CreateCardWeb(string idempotencyKey, PayInCardWebPostDTO payIn);

        /// <summary>Creates new payin by PayPal.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInPayPalDTO> CreatePayPal(PayInPayPalPostDTO payIn);

        /// <summary>Creates new payin by PayPal.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInPayPalDTO> CreatePayPal(string idempotencyKey, PayInPayPalPostDTO payIn);

        /// <summary>Creates new payin preauthorized direct.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInPreauthorizedDirectDTO> CreatePreauthorizedDirect(PayInPreauthorizedDirectPostDTO payIn);

        /// <summary>Creates new payin preauthorized direct.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInPreauthorizedDirectDTO> CreatePreauthorizedDirect(string idempotencyKey, PayInPreauthorizedDirectPostDTO payIn);

        /// <summary>Creates new payin direct debit.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInDirectDebitDTO> CreateDirectDebit(PayInDirectDebitPostDTO payIn);

        /// <summary>Creates new payin direct debit.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInDirectDebitDTO> CreateDirectDebit(string idempotencyKey, PayInDirectDebitPostDTO payIn);

        /// <summary>Creates new payin mandate direct debit.</summary>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInMandateDirectDTO> CreateMandateDirectDebit(PayInMandateDirectPostDTO payIn);

        /// <summary>Creates new payin mandate direct debit.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payIn">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInMandateDirectDTO> CreateMandateDirectDebit(string idempotencyKey, PayInMandateDirectPostDTO payIn);

        /// <summary>Gets PayIn entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInDTO> Get(string payInId);

        /// <summary>Gets PayIn bankwire direct entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInBankWireDirectDTO> GetBankWireDirect(string payInId);

        /// <summary>Gets PayIn bankwire external instruction entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInBankWireExternalInstructionDTO> GetBankWireExternalInstruction(string payInId);

        /// <summary>Gets PayIn card direct entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInCardDirectDTO> GetCardDirect(string payInId);

        /// <summary>Gets PayIn card web entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInCardWebDTO> GetCardWeb(string payInId);

        /// <summary>Gets limited card data for PayIn card web.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>Simplified Card object returned from API.</returns>
        Task<CardDTO> GetCardDataForCardWeb(string payInId);

        /// <summary>Gets PayIn preauthorized direct entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInPreauthorizedDirectDTO> GetPreauthorizedDirect(string payInId);

        /// <summary>Gets PayIn direct debit entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInDirectDebitDTO> GetDirectDebit(string payInId);

        /// <summary>Gets PayIn direct debit direct entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInMandateDirectDTO> GetMandateDirectDebit(string payInId);

        /// <summary>Gets PayIn PayPal entity by its identifier.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <returns>PayIn object returned from API.</returns>
        Task<PayInPayPalDTO> GetPayPal(string payInId);

        /// <summary>Creates refund for PayIn object.</summary>
        /// <param name="payInId">PayIn identifier.</param>
        /// <param name="refund">Refund object to be created.</param>
        /// <returns>Refund entity instance returned from API.</returns>
        Task<RefundDTO> CreateRefund(string payInId, RefundPayInPostDTO refund);

        /// <summary>Creates refund for PayIn object.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payInId">PayIn identifier.</param>
        /// <param name="refund">Refund object to be created.</param>
        /// <returns>Refund entity instance returned from API.</returns>
        Task<RefundDTO> CreateRefund(string idempotencyKey, string payInId, RefundPayInPostDTO refund);
    }
}