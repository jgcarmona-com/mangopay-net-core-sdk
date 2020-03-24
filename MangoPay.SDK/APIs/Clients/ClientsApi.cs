using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Clients
{
    public class ClientsApi : BaseApi, IClientsApi
    {
        public ClientsApi(
           MangoPayApiConfiguration config,
           ILogger<ClientsApi> logger,
           IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }
        /// <summary>
        /// ***Now deprecated and soon to be removed from this class (already moved to ApiKyc.cs)*** 
        /// Gets the list of all the uploaded documents for all users.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of all users' uploaded documents.</returns>
        public async Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(Pagination pagination, FilterKycDocuments filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/KYC/documents";
            if (filter == null) filter = new FilterKycDocuments();
            return await GetList<KycDocumentDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        /// <summary>
        /// Gets client wallets.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="pagination">Pagination.</param>
        /// <returns>Collection of client's wallets.</returns>
        public async Task<ListPaginated<WalletDTO>> GetWallets(FundsType fundsType, Pagination pagination)
        {
            switch (fundsType)
            {
                case FundsType.DEFAULT:
                    return await GetList<WalletDTO>($"{_baseUrl}/clients/wallets", pagination);
                case FundsType.FEES:
                    return await GetList<WalletDTO>($"{_baseUrl}/clients/wallets/fees", pagination);
                case FundsType.CREDIT:
                    return await GetList<WalletDTO>($"{_baseUrl}/clients/wallets/credit", pagination);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets client wallet.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="currency">Currency.</param>
        /// <returns>Wallet with given funds type and currency.</returns>
        public async Task<WalletDTO> GetWallet(FundsType fundsType, CurrencyIso currency)
        {
            if (currency == CurrencyIso.NotSpecified) return null;

            switch (fundsType)
            {
                case FundsType.DEFAULT:
                    return await GetEntity<WalletDTO>($"{_baseUrl}/clients/wallets/{currency.ToString()}");
                case FundsType.FEES:
                    return await GetEntity<WalletDTO>($"{_baseUrl}/clients/wallets/fees/{currency.ToString()}");
                case FundsType.CREDIT:
                    return await GetEntity<WalletDTO>($"{_baseUrl}/clients/wallets/credit/{currency.ToString()}");
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets client wallet transactions.
        /// </summary>
        /// <param name="fundsType">Type of funds.</param>
        /// <param name="currency">Currency.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns></returns>
        public async Task<ListPaginated<TransactionDTO>> GetWalletTransactions(FundsType fundsType, CurrencyIso currency, Pagination pagination, FilterTransactions filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/clients/wallets/{fundsType.ToString()}/{currency.ToString()}/transactions";
            if (filter == null) filter = new FilterTransactions();

            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        /// <summary>
        /// Gets client transactions.
        /// </summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of client's transactions.</returns>
        public async Task<ListPaginated<TransactionDTO>> GetTransactions(Pagination pagination, FilterTransactions filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/clients/transactions";
            if (filter == null) filter = new FilterTransactions();

            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        /// <summary>
        /// Creates new bankwire direct for client.
        /// </summary>
        /// <param name="bankWireDirect">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        public async Task<PayInBankWireDirectDTO> CreateBankWireDirect(ClientBankWireDirectPostDTO bankWireDirect)
        {
            var targetUrl = $"{_baseUrl}/clients/payins/bankwire/direct";
            return await CreateEntity<PayInBankWireDirectDTO, ClientBankWireDirectPostDTO>(targetUrl, bankWireDirect);
        }

        /// <summary>
        /// Creates new bankwire direct for client.
        /// </summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="bankWireDirect">Object instance to be created.</param>
        /// <returns>Object instance returned from API.</returns>
        public async Task<PayInBankWireDirectDTO> CreateBankWireDirect(string idempotencyKey, ClientBankWireDirectPostDTO bankWireDirect)
        {
            var targetUrl = $"{_baseUrl}/clients/payins/bankwire/direct";
            return await CreateEntity<PayInBankWireDirectDTO, ClientBankWireDirectPostDTO>(targetUrl, bankWireDirect, idempotencyKey);
        }

        /// <summary>
        /// Gets client entity.
        /// </summary>
        /// <returns>Object instance returned from API.</returns>
        public async Task<ClientDTO> Get()
        {
            var targetUrl = $"{_baseUrl}/clients";
            return await GetEntity<ClientDTO>(targetUrl);
        }

        /// <summary>
        /// Updates client information.
        /// </summary>
        /// <param name="client">Client entity instance to be updated.</param>
        /// <returns>Updated Client entity.</returns>
        public async Task<ClientDTO> Save(ClientPutDTO client)
        {
            var targetUrl = $"{_baseUrl}/clients";
            return await UpdateEntity<ClientDTO, ClientPutDTO>(targetUrl, client);
        }

        /// <summary>
        /// Uploads logo for client.
        /// </summary>
        /// <param name="binaryData">
        /// Binary file content (only GIF, PNG, JPG, JPEG, BMP, PDF and DOC formats are accepted).
        /// </param>
        public async Task UploadLogo(byte[] binaryData)
        {
            var targetUrl = $"{_baseUrl}/clients/logo";
            string fileContent = Convert.ToBase64String(binaryData);
            ClientLogoPutDTO logo = new ClientLogoPutDTO(fileContent);
            await UpdateEntity<ClientDTO, ClientLogoPutDTO>(targetUrl, logo);
            return;
        }

        /// <summary>
        /// Uploads logo for client.
        /// </summary>
        /// <param name="filePath">
        /// Path to logo file (only GIF, PNG, JPG, JPEG, BMP, PDF and DOC formats are accepted).
        /// </param>
        public async Task UploadLogo(string filePath)
        {
            byte[] fileArray = File.ReadAllBytes(filePath);
            await UploadLogo(fileArray);
        }
    }
}
