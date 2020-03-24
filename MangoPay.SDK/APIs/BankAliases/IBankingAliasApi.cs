using MangoPay.SDK.Core;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.BankAliases
{
    public interface IBankingAliasApi
    {
        /// <summary>Create an IBAN banking alias.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <param name="bankingAlias">IBAN banking alias instance to be created.</param>
        /// <returns>Banking alias object returned from API.</returns>
        Task<BankingAliasIbanDTO> CreateIban(string walletId, BankingAliasIbanPostDTO bankingAlias);

        /// <summary>Gets details of a banking alias.</summary>
        /// <param name="bankingAliasId">Banking alias identifier.</param>
        /// <returns>Banking alias object returned from API.</returns>
        Task<BankingAliasDTO> Get(string bankingAliasId);


        /// <summary>Gets details of a IBAN banking alias.</summary>
        /// <param name="bankingAliasId">Banking alias identifier.</param>
        /// <returns>IBAN banking alias object returned from API.</returns>
        Task<BankingAliasIbanDTO> GetIban(string bankingAliasId);

        /// <summary>Gets list of a banking aliases for a wallet.</summary>
        /// <param name="walletId">Wallet identifier.</param>
        /// <param name="pagination">Pagination.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Collection of banking aliases instances.</returns>
        Task<ListPaginated<BankingAliasDTO>> GetAll(string walletId, Pagination pagination, Sort sort = null);

        /// <summary>Updates bank account.</summary>
        /// <param name="bankingAlias">Banking alias instance to be updated.</param>
        /// <param name="bankingAliasId">Banking alias identifier.</param>
        /// <returns>Banking alias object returned from API.</returns>
        Task<BankingAliasDTO> Update(BankingAliasPutDTO bankingAlias, string bankingAliasId);
    }
}