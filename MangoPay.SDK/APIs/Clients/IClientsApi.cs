using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Clients
{
    public interface IClientsApi        
    {
        /// <summary>
        /// ***Now deprecated and soon to be removed from this class (already moved to ApiKyc.cs)*** 
        /// Gets the list of all the uploaded documents for all users.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of all users' uploaded documents.</returns>
        Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(Pagination pagination, FilterKycDocuments filter, Sort sort = null);

        /// <summary>
        /// Gets client wallets.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="pagination">Pagination.</param>
        /// <returns>Collection of client's wallets.</returns>
        Task<ListPaginated<WalletDTO>> GetWallets(FundsType fundsType, Pagination pagination);

        /// <summary>
        /// Gets client wallet.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="currency">Currency.</param>
        /// <returns>Wallet with given funds type and currency.</returns>
        Task<WalletDTO> GetWallet(FundsType fundsType, CurrencyIso currency);
        
        /// <summary>
        /// Gets client wallet transactions.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="currency">Currency.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns></returns>
        Task<ListPaginated<TransactionDTO>> GetWalletTransactions(FundsType fundsType, CurrencyIso currency, Pagination pagination, FilterTransactions filter, Sort sort = null);
               
        /// <summary>
        /// Gets client transactions.
        /// </summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of client's transactions.</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactions(Pagination pagination, FilterTransactions filter, Sort sort = null);

        /// <summary>
        /// Creates new bankwire direct for client.
        /// </summary>
        /// <param name="bankWireDirect">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInBankWireDirectDTO> CreateBankWireDirect(ClientBankWireDirectPostDTO bankWireDirect);

        /// <summary>
        /// Creates new bankwire direct for client.
        /// </summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="bankWireDirect">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        Task<PayInBankWireDirectDTO >CreateBankWireDirect(string idempotencyKey, ClientBankWireDirectPostDTO bankWireDirect);

        /// <summary>
        /// Gets client entity.
        /// </summary>
        /// <returns>Object instance returned from API.</returns>
        Task<ClientDTO> Get();

        /// <summary>
        /// Updates client information.
        /// </summary>
        /// <param name="client">Client entity instance to be updated.</param>
        /// <returns>Updated Client entity.</returns>
        Task< ClientDTO> Save(ClientPutDTO client);

        /// <summary>
        /// Uploads logo for client.
        /// </summary>
        /// <param name="binaryData">
        /// Binary file content (only GIF, PNG, JPG, JPEG, BMP, PDF and DOC formats are accepted).
        /// </param>
        Task UploadLogo(byte[] binaryData);

        /// <summary>
        /// Uploads logo for client.
        /// </summary>
        /// <param name="filePath">
        /// Path to logo file (only GIF, PNG, JPG, JPEG, BMP, PDF and DOC formats are accepted).
        /// </param>
        Task UploadLogo(string filePath);       
    }
}
