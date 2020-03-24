using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Transfers
{
    public interface ITransfersApi
    {
        /// <summary>Creates new transfer.</summary>
        /// <param name="transfer">Transfer entity instance to be created.</param>
        /// <returns>Transfer object returned from API.</returns>
        Task<TransferDTO> Create(TransferPostDTO transfer);

        /// <summary>Creates new transfer.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="transfer">Transfer entity instance to be created.</param>
        /// <returns>Transfer object returned from API.</returns>
        Task<TransferDTO> Create(string idempotencyKey, TransferPostDTO transfer);

        /// <summary>Gets the transfer.</summary>
        /// <param name="transferId">Transfer identifier.</param>
        /// <returns>Transfer instance returned from API.</returns>
        Task<TransferDTO> Get(string transferId);

        /// <summary>Creates refund for transfer object.</summary>
        /// <param name="transferId">Transfer identifier.</param>
        /// <param name="refund">Refund object to create.</param>
        /// <returns>Refund entity instance returned from API.</returns>
        Task<RefundDTO> CreateRefund(string transferId, RefundTransferPostDTO refund);

        /// <summary>Creates refund for transfer object.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="transferId">Transfer identifier.</param>
        /// <param name="refund">Refund object to create.</param>
        /// <returns>Refund entity instance returned from API.</returns>
        Task<RefundDTO> CreateRefund(string idempotencyKey, string transferId, RefundTransferPostDTO refund);
    }
}
