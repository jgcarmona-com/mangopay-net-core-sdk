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
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Users
{
    public class UserApi : BaseApi, IUsersApi
    {
        public UserApi(
            MangoPayApiConfiguration config,
            ILogger<UserApi> logger,
            IAuthApi auth) : base(config, logger, auth)
        {
            _baseUrl = $"{_config.BaseUrl}/{ _config.ApiVersion}/{ _config.ClientId}";
        }

        public async Task<UserDTO> Get(string userId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/";

            return await GetEntity<UserDTO>(targetUrl);

        }

        public async Task<UserNaturalDTO> Create(UserNaturalPostDTO user)
        {
            var targetUrl = $"{_baseUrl}/users/natural/";
            return await CreateEntity<UserNaturalDTO, UserNaturalPostDTO>(targetUrl, user);
        }

        public async Task<UserLegalDTO> Create(UserLegalPostDTO user)
        {
            var targetUrl = $"{_baseUrl}/users/legal/";
            return await CreateEntity<UserLegalDTO, UserLegalPostDTO>(targetUrl, user);
        }

        public Task<ListPaginated<UserDTO>> GetAll(Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users";
            return GetList<UserDTO>(targetUrl, pagination, sort);
        }

        public Task<ListPaginated<UserDTO>> GetAll()
        {
            return GetAll(null);
        }

        public async Task<UserNaturalDTO> GetNatural(string userId)
        {
            var targetUrl = $"{_baseUrl}/users/natural/{userId}";

            return await GetEntity<UserNaturalDTO>(targetUrl);
        }

        public async Task<UserLegalDTO> GetLegal(string userId)
        {
            var targetUrl = $"{_baseUrl}/users/legal/{userId}";
            return await GetEntity<UserLegalDTO>(targetUrl);
        }

        public async Task<UserNaturalDTO> UpdateNatural(UserNaturalPutDTO user, string userId)
        {
            var targetUrl = $"{_baseUrl}/users/natural/{userId}";
            return await UpdateEntity<UserNaturalDTO, UserNaturalPutDTO>(targetUrl, user);
        }

        public async Task<UserLegalDTO> UpdateLegal(UserLegalPutDTO user, string userId)
        {
            var targetUrl = $"{_baseUrl}/users/legal/{userId}";
            return await UpdateEntity<UserLegalDTO, UserLegalPutDTO>(targetUrl, user);
        }

        public async Task<ListPaginated<WalletDTO>> GetWallets(string userId, Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/wallets";
            return await GetList<WalletDTO>(targetUrl, pagination, sort);
        }

        public async Task<BankAccountCaDTO> CreateBankAccountCa(string userId, BankAccountCaPostDTO bankAccount)
        {
            return await CreateBankAccountCa(null, userId, bankAccount);
        }

        public async Task<BankAccountCaDTO> CreateBankAccountCa(string idempotencyKey, string userId, BankAccountCaPostDTO bankAccount)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/ca";
            return await CreateEntity<BankAccountCaDTO, BankAccountCaPostDTO>(targetUrl, bankAccount, idempotencyKey);
        }

        public async Task<BankAccountGbDTO> CreateBankAccountGb(string userId, BankAccountGbPostDTO bankAccount)
        {
            return await CreateBankAccountGb(null, userId, bankAccount);
        }

        public async Task<BankAccountGbDTO> CreateBankAccountGb(string idempotencyKey, string userId, BankAccountGbPostDTO bankAccount)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/gb";
            return await CreateEntity<BankAccountGbDTO, BankAccountGbPostDTO>(targetUrl, bankAccount, idempotencyKey);
        }

        public async Task<BankAccountIbanDTO> CreateBankAccountIban(string userId, BankAccountIbanPostDTO bankAccount)
        {
            return await CreateBankAccountIban(null, userId, bankAccount);
        }

        public async Task<BankAccountIbanDTO> CreateBankAccountIban(string idempotencyKey, string userId, BankAccountIbanPostDTO bankAccount)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/iban";
            return await CreateEntity<BankAccountIbanDTO, BankAccountIbanPostDTO>(targetUrl, bankAccount);
        }

        public async Task<BankAccountOtherDTO> CreateBankAccountOther(string userId, BankAccountOtherPostDTO bankAccount)
        {
            return await CreateBankAccountOther(null, userId, bankAccount);
        }

        public async Task<BankAccountOtherDTO> CreateBankAccountOther(string idempotencyKey, string userId, BankAccountOtherPostDTO bankAccount)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/other";
            return await CreateEntity<BankAccountOtherDTO, BankAccountOtherPostDTO>(targetUrl, bankAccount, idempotencyKey);
        }

        public async Task<BankAccountUsDTO> CreateBankAccountUs(string userId, BankAccountUsPostDTO bankAccount)
        {
            return await CreateBankAccountUs(null, userId, bankAccount);
        }

        public async Task<BankAccountUsDTO> CreateBankAccountUs(string idempotencyKey, string userId, BankAccountUsPostDTO bankAccount)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/us";
            return await CreateEntity<BankAccountUsDTO, BankAccountUsPostDTO>(targetUrl, bankAccount, idempotencyKey);
        }

        public async Task<ListPaginated<BankAccountDTO>> GetBankAccounts(string userId, Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts";
            return await GetList<BankAccountDTO>(targetUrl, pagination, sort);
        }

        public async Task<ListPaginated<BankAccountDTO>> GetBankAccounts(string userId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts";
            return await GetList<BankAccountDTO>(targetUrl, null);
        }

        public async Task<BankAccountDTO> GetBankAccount(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountDTO>(targetUrl);
        }

        public async Task<BankAccountCaDTO> GetBankAccountCa(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountCaDTO>(targetUrl);
        }

        public async Task<BankAccountGbDTO> GetBankAccountGb(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountGbDTO>(targetUrl);
        }

        public async Task<BankAccountIbanDTO> GetBankAccountIban(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountIbanDTO>(targetUrl);
        }

        public async Task<BankAccountOtherDTO> GetBankAccountOther(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountOtherDTO>(targetUrl);
        }

        public async Task<BankAccountUsDTO> GetBankAccountUs(string userId, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await GetEntity<BankAccountUsDTO>(targetUrl);
        }

        public async Task<BankAccountDTO> UpdateBankAccount(string userId, DisactivateBankAccountPutDTO bankAccount, string bankAccountId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/bankaccounts/{bankAccountId}";
            return await UpdateEntity<BankAccountDTO, DisactivateBankAccountPutDTO>(targetUrl, bankAccount);
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactionsForBankAccount(string bankAccountId, Pagination pagination, FilterTransactions filters, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/bankaccounts/{bankAccountId}/transactions";
            if (filters == null) filters = new FilterTransactions();
            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filters.GetValues());
        }

        public async Task<ListPaginated<TransactionDTO>> GetTransactions(string userId, Pagination pagination, FilterTransactions filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/transactions";
            return await GetList<TransactionDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        public async Task<ListPaginated<CardDTO>> GetCards(string userId, Pagination pagination, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/cards";
            return await GetList<CardDTO>(targetUrl, pagination, sort);
        }

        public Task CreateKycPage(string userId, string kycDocumentId, byte[] binaryData)
        {
            return CreateKycPage(null, userId, kycDocumentId, binaryData);
        }

        public Task CreateKycPage(string idempotencyKey, string userId, string kycDocumentId, byte[] binaryData)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/KYC/documents/{kycDocumentId}/pages";
            var fileContent = Convert.ToBase64String(binaryData);

            var kycPage = new KycPagePostDTO(fileContent);

            return CreateEntity<KycPageDTO, KycPagePostDTO>(targetUrl, kycPage, idempotencyKey);
        }

        public Task CreateKycPage(string userId, string kycDocumentId, string filePath)
        {
            return CreateKycPage(null, userId, kycDocumentId, filePath);
        }

        public Task CreateKycPage(string idempotencyKey, string userId, string kycDocumentId, string filePath)
        {
            byte[] fileArray = File.ReadAllBytes(filePath);
            return CreateKycPage(idempotencyKey, userId, kycDocumentId, fileArray);
        }

        public async Task<KycDocumentDTO> CreateKycDocument(string userId, KycDocumentType type, string tag = null)
        {
            return await CreateKycDocument(null, userId, type, tag);
        }

        public async Task<KycDocumentDTO> CreateKycDocument(string idempotencyKey, string userId, KycDocumentType type, string tag = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/KYC/documents";
            KycDocumentPostDTO kycDocument = new KycDocumentPostDTO(type);
            kycDocument.Tag = tag;

            return await CreateEntity<KycDocumentDTO, KycDocumentPostDTO>(targetUrl, kycDocument, idempotencyKey);
        }

        public async Task<KycDocumentDTO> GetKycDocument(string userId, string kycDocumentId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/KYC/documents/{kycDocumentId}";
            return await GetEntity<KycDocumentDTO>(targetUrl);

        }

        public async Task<KycDocumentDTO> UpdateKycDocument(string userId, KycDocumentPutDTO kycDocument, string kycDocumentId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/KYC/documents/{kycDocumentId}";
            return await UpdateEntity<KycDocumentDTO, KycDocumentPutDTO>(targetUrl, kycDocument);
        }

        public async Task<ListPaginated<KycDocumentDTO>> GetKycDocuments(string userId, Pagination pagination, FilterKycDocuments filter, Sort sort = null)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/KYC/documents";
            if (filter == null) filter = new FilterKycDocuments();
            return await GetList<KycDocumentDTO>(targetUrl, pagination, sort, filter.GetValues());
        }

        public async Task<EmoneyDTO> GetEmoney(string userId)
        {
            return await GetEmoney(userId, CurrencyIso.NotSpecified);
        }

        public async Task<EmoneyDTO> GetEmoney(string userId, CurrencyIso currency)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/emoney";
            if (currency != CurrencyIso.NotSpecified)
            {
                targetUrl += "?currency=" + currency.ToString();
            }

            return await GetEntity<EmoneyDTO>(targetUrl);
        }

        public async Task<ListPaginated<WalletDTO>> GetWallets(string userId)
        {
            var targetUrl = $"{_baseUrl}/users/{userId}/wallets";

            return await GetList<WalletDTO>(targetUrl, null);
        }


    }
}
