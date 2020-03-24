using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Users
{
    public interface IUsersApi
    {
        /// <summary>
        /// Gets the user by its identifier.
        /// </summary>
        /// <param name="userId">The user.</param>
        /// <returns></returns>
        Task<UserDTO> Get(string userId);
        Task<UserNaturalDTO> Create(UserNaturalPostDTO user);
        Task<UserLegalDTO> Create(UserLegalPostDTO user);
        Task<ListPaginated<UserDTO>> GetAll();
        Task<ListPaginated<UserDTO>> GetAll(Pagination pagination, Sort sort = null);
        Task<UserNaturalDTO> GetNatural(string userId);
        Task<UserLegalDTO> GetLegal(string userId);
        Task<UserNaturalDTO> UpdateNatural(UserNaturalPutDTO user, string userId);
        Task<UserLegalDTO> UpdateLegal(UserLegalPutDTO user, string userId);
        Task<ListPaginated<WalletDTO>> GetWallets(string userId, Pagination pagination, Sort sort = null);
        Task<BankAccountCaDTO> CreateBankAccountCa(string userId, BankAccountCaPostDTO bankAccount);
        Task<BankAccountCaDTO> CreateBankAccountCa(string idempotencyKey, string userId, BankAccountCaPostDTO bankAccount);
        Task<BankAccountGbDTO> CreateBankAccountGb(string userId, BankAccountGbPostDTO bankAccount);
        Task<BankAccountGbDTO> CreateBankAccountGb(string idempotencyKey, string userId, BankAccountGbPostDTO bankAccount);
        Task<BankAccountIbanDTO> CreateBankAccountIban(string userId, BankAccountIbanPostDTO bankAccount);
        Task<BankAccountIbanDTO> CreateBankAccountIban(string idempotencyKey, string userId, BankAccountIbanPostDTO bankAccount);
        Task<BankAccountOtherDTO> CreateBankAccountOther(string userId, BankAccountOtherPostDTO bankAccount);
        Task<BankAccountOtherDTO> CreateBankAccountOther(string idempotencyKey, string userId, BankAccountOtherPostDTO bankAccount);
        Task<BankAccountUsDTO> CreateBankAccountUs(string idempotencyKey, string userId, BankAccountUsPostDTO bankAccount);
        Task<BankAccountUsDTO> CreateBankAccountUs(string userId, BankAccountUsPostDTO bankAccount);
        Task<ListPaginated<BankAccountDTO>> GetBankAccounts(string userId, Pagination pagination, Sort sort = null);
        Task<ListPaginated<BankAccountDTO>> GetBankAccounts(string userId);
        Task<BankAccountDTO> GetBankAccount(string userId, string bankAccountId);
        Task<BankAccountCaDTO> GetBankAccountCa(string userId, string bankAccountId);
        Task<BankAccountGbDTO> GetBankAccountGb(string userId, string bankAccountId);
        Task<BankAccountIbanDTO> GetBankAccountIban(string userId, string bankAccountId);
        Task<BankAccountOtherDTO> GetBankAccountOther(string userId, string bankAccountId);
        Task<BankAccountUsDTO> GetBankAccountUs(string userId, string bankAccountId);
        Task<BankAccountDTO> UpdateBankAccount(string userId, DisactivateBankAccountPutDTO bankAccount, string bankAccountId);
        Task<ListPaginated<TransactionDTO>> GetTransactionsForBankAccount(string bankAccountId, Pagination pagination, FilterTransactions filters, Sort sort = null);
        Task<ListPaginated<TransactionDTO>> GetTransactions(string userId, Pagination pagination, FilterTransactions filter, Sort sort = null);
        Task<ListPaginated<CardDTO>> GetCards(string userId, Pagination pagination, Sort sort = null);
        Task CreateKycPage(string userId, string kycDocumentId, byte[] binaryData);
        Task CreateKycPage(string idempotencyKey, string userId, string kycDocumentId, byte[] binaryData);
        Task CreateKycPage(string userId, string kycDocumentId, string filePath);
        Task CreateKycPage(string idempotencyKey, string userId, string kycDocumentId, string filePath);
        Task<KycDocumentDTO> CreateKycDocument(string userId, KycDocumentType type, string tag = null);
        Task<KycDocumentDTO> CreateKycDocument(string idempotencyKey, string userId, KycDocumentType type, string tag = null);
        Task<KycDocumentDTO> GetKycDocument(string userId, string kycDocumentId);
        Task<KycDocumentDTO> UpdateKycDocument(string userId, KycDocumentPutDTO kycDocument, string kycDocumentId);
        Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(string userId, Pagination pagination, FilterKycDocuments filter, Sort sort = null);
        Task<EmoneyDTO> GetEmoney(string userId);
        Task<EmoneyDTO> GetEmoney(string userId, CurrencyIso currency);
        Task<ListPaginated<WalletDTO>> GetWallets(string userId);
    }
}
