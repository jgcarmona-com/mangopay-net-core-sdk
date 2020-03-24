using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Wallets
{
    public interface IWalletsApi
    {
        /// <summary>Creates new wallet.</summary>
        /// <param name="wallet">Wallet instance to be created.</param>
        /// <returns>Wallet instance returned from API.</returns>
        Task<WalletDTO> Create(WalletPostDTO wallet);

        /// <summary>Creates new wallet.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="wallet">Wallet instance to be created.</param>
        /// <returns>Wallet instance returned from API.</returns>
        Task<WalletDTO> Create(string idempotencyKey, WalletPostDTO wallet);

        /// <summary>Gets wallet.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <returns>Wallet instance returned from API.</returns>
        Task<WalletDTO> Get(string walletId);

        /// <summary>Updates wallet.</summary>
        /// <param name="wallet">Wallet object to save.</param>
        /// <param name="walletId">Wallet identifier.</param>
        /// <returns>Wallet instance returned from API.</returns>
        Task<WalletDTO> Update(WalletPutDTO wallet, string walletId);

        /// <summary>Gets transactions for the wallet.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Transactions for wallet returned from API.</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId, Pagination pagination, FilterTransactions filter, Sort sort = null);

        /// <summary>Gets transactions for the wallet.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Transactions for wallet returned from API.</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId, Pagination pagination, Sort sort = null);

        /// <summary>Gets transactions for the wallet.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <returns>Transactions for wallet returned from API.</returns>
        Task<ListPaginated<TransactionDTO>> GetTransactions(string walletId);
    }
}


