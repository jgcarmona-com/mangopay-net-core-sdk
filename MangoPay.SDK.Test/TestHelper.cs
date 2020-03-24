using MangoPay.SDK.APIs.CardPreAuthorizations;
using MangoPay.SDK.APIs.CardRegistrations;
using MangoPay.SDK.APIs.Cards;
using MangoPay.SDK.APIs.Hooks;
using MangoPay.SDK.APIs.Mandates;
using MangoPay.SDK.APIs.PayIns;
using MangoPay.SDK.APIs.PayOuts;
using MangoPay.SDK.APIs.Reports;
using MangoPay.SDK.APIs.Transfers;
using MangoPay.SDK.APIs.Users;
using MangoPay.SDK.APIs.Wallets;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace MangoPay.SDK.Test
{
    public static class TestHelper
    {
        private static readonly ServiceCollection _services = new ServiceCollection();
        private static readonly IConfigurationRoot _configuration = TestHelper.GetIConfigurationRoot();
        private static ServiceProvider _provider;
        private static IUsersApi _usersApi;
        private static IWalletsApi _walletsApi;
        private static IHooksApi _hooksApi;
        private static IPayInsApi _payInsApi;
        private static IPayOutsApi _payOutsApi;
        private static ITransfersApi _transfersApi;
        private static IReportsApi _reportsApi;
        private static IMandatesApi _mandatesApi;
        private static ICardRegistrationsApi _cardRegistrationsApi;
        private static ICardPreAuthorizationsApi _cardPreAuthorizationsApi;
        private static UserNaturalDTO _john;
        private static UserLegalDTO _matrix;
        private static BankAccountIbanDTO _johnsAccount;
        private static WalletDTO _johnsWallet;
        private static WalletDTO _johnsWalletWithMoney;
        private static PayInCardWebDTO _johnsPayInCardWeb;
        private static PayOutBankWireDTO _johnsPayOutBankWire;
        private static CardRegistrationDTO _johnsCardRegistration;
        private static KycDocumentDTO _johnsKycDocument;
        private static PayOutBankWireDTO _johnsPayOutForCardDirect;
        private static HookDTO _johnsHook;
        private static Dictionary<ReportType, ReportRequestDTO> _johnsReports;

        public static ICardsApi CardsApi { get; set; }
        public static IWalletsApi WalletsApi { get => _walletsApi; set => _walletsApi = value; }

        static TestHelper()
        {
            _services = new ServiceCollection();
            _configuration = GetIConfigurationRoot();
            _services.AddMangoPayServices(_configuration);
            _provider = _services.BuildServiceProvider();
            _usersApi = _provider.GetRequiredService<IUsersApi>();
            WalletsApi = _provider.GetRequiredService<IWalletsApi>();
            _hooksApi = _provider.GetRequiredService<IHooksApi>();
            CardsApi = _provider.GetRequiredService<ICardsApi>();
            _payInsApi = _provider.GetRequiredService<IPayInsApi>();
            _payOutsApi = _provider.GetRequiredService<IPayOutsApi>();
            _transfersApi = _provider.GetRequiredService<ITransfersApi>();
            _reportsApi = _provider.GetRequiredService<IReportsApi>();
            _mandatesApi = _provider.GetRequiredService<IMandatesApi>();
            _cardPreAuthorizationsApi = _provider.GetRequiredService<ICardPreAuthorizationsApi>();
            _cardRegistrationsApi = _provider.GetRequiredService<ICardRegistrationsApi>();

            _johnsReports = new Dictionary<ReportType, ReportRequestDTO>();
        }

        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        public static MangoPayApiConfiguration GetApplicationConfiguration()
        {
            var configuration = new MangoPayApiConfiguration();

            var iConfig = GetIConfigurationRoot();

            iConfig
                .GetSection("MangoPayApiConfiguration")
                .Bind(configuration);

            return configuration;
        }

        public static UserNaturalDTO GetJohn(bool recreate = false)
        {
            if (_john == null || recreate)
            {
                UserNaturalPostDTO user = new UserNaturalPostDTO("john.doe@sample.org", "John", "Doe", new DateTime(1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso.FR);
                user.Occupation = "programmer";
                user.IncomeRange = 3;
                user.Address = new Address { AddressLine1 = "Address line 1", AddressLine2 = "Address line 2", City = "City", Country = CountryIso.PL, PostalCode = "11222", Region = "Region" };
                user.Capacity = CapacityType.DECLARATIVE;

                _john = _usersApi.Create(user).Result;

                _johnsWallet = null;
            }
            return _john;
        }
        public static UserNaturalDTO GetNewJohn()
        {
            UserNaturalPostDTO user = new UserNaturalPostDTO("john.doe@sample.org", "John", "Doe", new DateTime(1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso.FR);
            user.Occupation = "programmer";
            user.IncomeRange = 3;
            user.Address = new Address { AddressLine1 = "Address line 1", AddressLine2 = "Address line 2", City = "City", Country = CountryIso.PL, PostalCode = "11222", Region = "Region" };
            user.Capacity = CapacityType.DECLARATIVE;

            return _usersApi.Create(user).Result;
        }

        public static UserLegalDTO GetMatrix()
        {
            if (_matrix == null)
            {
                UserNaturalDTO john = GetJohn();
                var birthday = john.Birthday.HasValue ? john.Birthday.Value : new DateTime();
                UserLegalPostDTO user = new UserLegalPostDTO(john.Email, "MartixSampleOrg","6747", LegalPersonType.BUSINESS, john.FirstName, john.LastName, birthday, john.Nationality, john.CountryOfResidence);
                user.HeadquartersAddress = new Address { AddressLine1 = "Address line 1", AddressLine2 = "Address line 2", City = "City", Country = CountryIso.PL, PostalCode = "11222", Region = "Region" };
                user.LegalRepresentativeAddress = john.Address;
                user.LegalRepresentativeEmail = john.Email;
                user.LegalRepresentativeBirthday = new DateTime(1975, 12, 21, 0, 0, 0);
                user.Email = john.Email;

                _matrix = _usersApi.Create(user).Result;
            }
            return _matrix;
        }

        public static BankAccountIbanDTO GetJohnsAccount(bool recreate = false)
        {
            if (_johnsAccount == null || recreate)
            {
                UserNaturalDTO john = GetJohn();
                BankAccountIbanPostDTO account = new BankAccountIbanPostDTO(john.FirstName + " " + john.LastName, john.Address, "FR7618829754160173622224154");
                account.UserId = john.Id;
                account.BIC = "CMBRFR2BCME";
                _johnsAccount = _usersApi.CreateBankAccountIban(john.Id, account).Result;
            }
            return _johnsAccount;
        }

        public static WalletDTO GetJohnsWallet()
        {
            if (_johnsWallet == null)
            {
                UserNaturalDTO john = GetJohn();

                WalletPostDTO wallet = new WalletPostDTO(new List<string> { john.Id }, "WALLET IN EUR", CurrencyIso.EUR);

                _johnsWallet = WalletsApi.Create(wallet).Result;
            }

            return _johnsWallet;
        }


        public static WalletDTO CreateJohnsWallet()
        {

            UserNaturalDTO john = GetJohn();

            WalletPostDTO wallet = new WalletPostDTO(new List<string> { john.Id }, "WALLET IN EUR", CurrencyIso.EUR);

            return WalletsApi.Create(wallet).Result;

        }

        /// <summary>Creates wallet for John, loaded with 10k EUR (John's got lucky) if not created yet, or returns an existing one.</summary>
        /// <returns>Wallet instance loaded with 10k EUR.</returns>
        public static WalletDTO GetJohnsWalletWithMoney()
        {
            return GetJohnsWalletWithMoney(10000);
        }

        /// <summary>Creates wallet for John, if not created yet, or returns an existing one.</summary>
        /// <param name="amount">Initial wallet's money amount.</param>
        /// <returns>Wallet entity instance returned from API.</returns>
        static WalletDTO GetJohnsWalletWithMoney(int amount)
        {
            if (_johnsWalletWithMoney == null)
                _johnsWalletWithMoney = GetNewJohnsWalletWithMoney(amount);

            return _johnsWalletWithMoney;
        }

        /// <summary>Creates new wallet for John.</summary>
        /// <param name="amount">Initial wallet's money amount.</param>
        /// <returns>Wallet entity instance returned from API.</returns>
        public static WalletDTO GetNewJohnsWalletWithMoney(int amount, UserNaturalDTO user = null)
        {
            UserNaturalDTO john = user;
            if (john == null)
                john = GetJohn();

            // create wallet with money
            WalletPostDTO wallet = new WalletPostDTO(new List<string> { john.Id }, "WALLET IN EUR WITH MONEY", CurrencyIso.EUR);

            var johnsWalletWithMoney = WalletsApi.Create(wallet).Result;

            CardRegistrationPostDTO cardRegistrationPost = new CardRegistrationPostDTO(johnsWalletWithMoney.Owners[0], CurrencyIso.EUR);
            CardRegistrationDTO cardRegistration = _cardRegistrationsApi.Create(cardRegistrationPost).Result;

            CardRegistrationPutDTO cardRegistrationPut = new CardRegistrationPutDTO();
            cardRegistrationPut.RegistrationData = GetPaylineCorrectRegistartionData(cardRegistration);
            cardRegistration = _cardRegistrationsApi.Update(cardRegistrationPut, cardRegistration.Id).Result;

            CardDTO card = CardsApi.Get(cardRegistration.CardId).Result;

            // create pay-in CARD DIRECT
            PayInCardDirectPostDTO payIn = new PayInCardDirectPostDTO(cardRegistration.UserId, cardRegistration.UserId,
                new Money { Amount = amount, Currency = CurrencyIso.EUR },
                new Money { Amount = 0, Currency = CurrencyIso.EUR },
                johnsWalletWithMoney.Id, "http://test.com", card.Id)
            {
                CardType = card.CardType
            };

            // create Pay-In
            var payin = _payInsApi.CreateCardDirect(payIn).Result;

            return WalletsApi.Get(johnsWalletWithMoney.Id).Result;
        }

        public static PayInCardWebDTO GetJohnsPayInCardWeb()
        {
            if (_johnsPayInCardWeb == null)
            {
                WalletDTO wallet = GetJohnsWallet();
                UserNaturalDTO user = GetJohn();

                PayInCardWebPostDTO payIn = new PayInCardWebPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "https://test.com", CultureCode.FR, CardType.CB_VISA_MASTERCARD);

                _johnsPayInCardWeb = _payInsApi.CreateCardWeb(payIn).Result;
            }

            return _johnsPayInCardWeb;
        }

        static PayInCardWebDTO GetJohnsNewPayInCardWeb()
        {
            _johnsPayInCardWeb = null;

            return GetJohnsPayInCardWeb();
        }

        static PayInCardWebDTO GetJohnsPayInCardWeb(string walletId)
        {
            if (_johnsPayInCardWeb == null)
            {
                UserNaturalDTO user = GetJohn();

                PayInCardWebPostDTO payIn = new PayInCardWebPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, walletId, "https://test.com", CultureCode.FR, CardType.CB_VISA_MASTERCARD);

                _johnsPayInCardWeb = _payInsApi.CreateCardWeb(payIn).Result;
            }

            return _johnsPayInCardWeb;
        }

        public static PayInCardWebDTO CreateJohnsPayInCardWeb(string walletId)
        {

            UserNaturalDTO user = GetJohn();

            PayInCardWebPostDTO payIn = new PayInCardWebPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, walletId, "https://test.com", CultureCode.FR, CardType.CB_VISA_MASTERCARD);

            return _payInsApi.CreateCardWeb(payIn).Result;
        }

        public static PayInCardWebDTO GetNewPayInCardWeb()
        {
            WalletDTO wallet = GetJohnsWallet();
            UserNaturalDTO user = GetJohn();

            PayInCardWebPostDTO payIn = new PayInCardWebPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "https://test.com", CultureCode.FR, CardType.CB_VISA_MASTERCARD);

            _johnsPayInCardWeb = _payInsApi.CreateCardWeb(payIn).Result;

            return _johnsPayInCardWeb;
        }

        public static PayInCardDirectDTO GetNewPayInCardDirect()
        {
            return GetNewPayInCardDirect(null);
        }
        public static PayInCardDirectDTO GetNewPayInCardDirectWithBilling()
        {
            return GetNewPayInCardDirectWithBilling(null);
        }

        /// <summary>Creates PayIn Card Direct object.</summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>PayIn Card Direct instance returned from API.</returns>
        static PayInCardDirectDTO GetNewPayInCardDirect(string userId)
        {
            return GetNewPayInCardDirect(userId, null);
        }
        /// <summary>Creates PayIn Card Direct object with billing information.</summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>PayIn Card Direct instance returned from API.</returns>
        static PayInCardDirectDTO GetNewPayInCardDirectWithBilling(string userId)
        {
            return GetNewPayInCardDirectWithBilling(userId, null);
        }

        /// <summary>Creates PayIn Card Direct object.</summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>PayIn Card Direct instance returned from API.</returns>
        static PayInCardDirectDTO GetNewPayInCardDirect(string userId, string idempotencyKey)
        {
            PayInCardDirectPostDTO payIn = GetPayInCardDirectPost(userId, idempotencyKey);
            return _payInsApi.CreateCardDirect(payIn).Result;
        }

        /// <summary>Creates PayIn Card Direct object with billing details.</summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>PayIn Card Direct instance returned from API.</returns>
        static PayInCardDirectDTO GetNewPayInCardDirectWithBilling(string userId, string idempotencyKey)
        {

            PayInCardDirectPostDTO payIn = GetPayInCardDirectPost(userId, idempotencyKey);
            Billing billing = new Billing();
            Address address = new Address();
            address.AddressLine1 = "Test address line 1";
            address.AddressLine2 = "Test address line 2";
            address.City = "Test city";
            address.Country = CountryIso.RO;
            address.PostalCode = "65400";
            billing.Address = address;
            payIn.Billing = billing;

            return _payInsApi.CreateCardDirect(payIn).Result;
        }

        static PayInCardDirectPostDTO GetPayInCardDirectPost(string userId, string idempotencyKey)
        {
            WalletDTO wallet = GetJohnsWalletWithMoney();

            if (userId == null)
            {
                UserNaturalDTO user = GetJohn();
                userId = user.Id;
            }

            CardRegistrationPostDTO cardRegistrationPost = new CardRegistrationPostDTO(userId, CurrencyIso.EUR);

            CardRegistrationDTO cardRegistration = _cardRegistrationsApi.Create(idempotencyKey, cardRegistrationPost).Result;

            CardRegistrationPutDTO cardRegistrationPut = new CardRegistrationPutDTO();
            cardRegistrationPut.RegistrationData = GetPaylineCorrectRegistartionData(cardRegistration);
            cardRegistration = _cardRegistrationsApi.Update(cardRegistrationPut, cardRegistration.Id).Result;

            CardDTO card = CardsApi.Get(cardRegistration.CardId).Result;

            // create pay-in CARD DIRECT
            PayInCardDirectPostDTO payIn = new PayInCardDirectPostDTO(cardRegistration.UserId, cardRegistration.UserId,
                    new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR },
                    wallet.Id, "http://test.com", card.Id);

            // payment type as CARD
            payIn.CardType = card.CardType;

            return payIn;
        }

        public static PayOutBankWireDTO GetJohnsPayOutBankWire()
        {
            if (_johnsPayOutBankWire == null)
            {
                WalletDTO wallet = GetJohnsWalletWithMoney();
                UserNaturalDTO user = GetJohn();
                BankAccountDTO account = GetJohnsAccount();

                PayOutBankWirePostDTO payOut = new PayOutBankWirePostDTO(user.Id, wallet.Id, new Money { Amount = 10, Currency = CurrencyIso.EUR }, new Money { Amount = 5, Currency = CurrencyIso.EUR }, account.Id, "Johns bank wire ref");
                payOut.Tag = "DefaultTag";
                payOut.CreditedUserId = user.Id;

                _johnsPayOutBankWire = _payOutsApi.CreateBankWire(payOut).Result;
            }

            return _johnsPayOutBankWire;
        }

        /// <summary>Creates PayOut Bank Wire object.</summary>
        /// <returns>PayOut Bank Wire instance returned from API.</returns>
        static PayOutBankWireDTO GetJohnsPayOutForCardDirect()
        {
            if (_johnsPayOutForCardDirect == null)
            {
                PayInCardDirectDTO payIn = GetNewPayInCardDirect();
                BankAccountDTO account = GetJohnsAccount();

                PayOutBankWirePostDTO payOut = new PayOutBankWirePostDTO(payIn.AuthorId, payIn.CreditedWalletId, new Money { Amount = 10, Currency = CurrencyIso.EUR },
                    new Money { Amount = 5, Currency = CurrencyIso.EUR }, account.Id, "Johns bank wire ref");
                payOut.Tag = "DefaultTag";
                payOut.CreditedUserId = payIn.AuthorId;

                _johnsPayOutForCardDirect = _payOutsApi.CreateBankWire(payOut).Result;
            }

            return _johnsPayOutForCardDirect;
        }

        public static TransferDTO GetNewTransfer(WalletDTO walletIn = null)
        {
            WalletDTO walletWithMoney = walletIn;
            if (walletWithMoney == null)
                walletWithMoney = GetJohnsWalletWithMoney();

            UserNaturalDTO user = GetJohn();
            WalletPostDTO walletPost = new WalletPostDTO(new List<string> { user.Id }, "WALLET IN EUR FOR TRANSFER", CurrencyIso.EUR);
            WalletDTO wallet = WalletsApi.Create(walletPost).Result;

            TransferPostDTO transfer = new TransferPostDTO(user.Id, user.Id, new Money { Amount = 100, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, walletWithMoney.Id, wallet.Id);
            transfer.Tag = "DefaultTag";

            return _transfersApi.Create(transfer).Result;
        }

        /// <summary>Creates refund object for transfer.</summary>
        /// <param name="transfer">Transfer.</param>
        /// <returns>Refund instance returned from API.</returns>
        public static RefundDTO GetNewRefundForTransfer(TransferDTO transfer)
        {
            UserNaturalDTO user = GetJohn();

            RefundTransferPostDTO refund = new RefundTransferPostDTO(user.Id);

            return _transfersApi.CreateRefund(transfer.Id, refund).Result;
        }

        /// <summary>Creates refund object for PayIn.</summary>
        /// <param name="payIn">PayIn entity.</param>
        /// <returns>Refund instance returned from API.</returns>
        public static RefundDTO GetNewRefundForPayIn(PayInDTO payIn)
        {
            return GetNewRefundForPayIn(payIn, null);
        }

        /// <summary>Creates refund object for PayIn.</summary>
        /// <param name="payIn">PayIn entity.</param>
        /// <returns>Refund instance returned from API.</returns>
        static RefundDTO GetNewRefundForPayIn(PayInDTO payIn, string idempotencyKey)
        {
            UserNaturalDTO user = GetJohn();

            Money debitedFunds = new Money();
            debitedFunds.Amount = payIn.DebitedFunds.Amount;
            debitedFunds.Currency = payIn.DebitedFunds.Currency;
            Money fees = new Money();
            fees.Amount = payIn.Fees.Amount;
            fees.Currency = payIn.Fees.Currency;

            RefundPayInPostDTO refund = new RefundPayInPostDTO(user.Id, fees, debitedFunds);
            return _payInsApi.CreateRefund(idempotencyKey, payIn.Id, refund).Result;
        }

        /// <summary>Creates card registration object.</summary>
        /// <param name="cardType">Card type.</param>
        /// <returns>CardRegistration instance returned from API.</returns>
        static CardRegistrationDTO GetJohnsCardRegistration(CardType cardType = CardType.CB_VISA_MASTERCARD)
        {
            if (_johnsCardRegistration == null)
            {
                UserNaturalDTO user = GetJohn();

                CardRegistrationPostDTO cardRegistration = new CardRegistrationPostDTO(user.Id, CurrencyIso.EUR, cardType);
                cardRegistration.Tag = "DefaultTag";

                _johnsCardRegistration = _cardRegistrationsApi.Create(cardRegistration).Result;
            }

            return _johnsCardRegistration;
        }

        /// <summary>Creates new card registration object.</summary>
        /// <param name="cardType">Card type.</param>
        /// <returns>CardRegistration instance returned from API.</returns>
        static CardRegistrationDTO GetNewJohnsCardRegistration(CardType cardType = CardType.CB_VISA_MASTERCARD)
        {
            _johnsCardRegistration = null;

            return GetJohnsCardRegistration(cardType);
        }

        /// <summary>Creates card registration object.</summary>
        /// <returns>CardPreAuthorization instance returned from API.</returns>
        public static CardPreAuthorizationDTO GetJohnsCardPreAuthorization()
        {
            return GetJohnsCardPreAuthorization(null);
        }

        /// <summary>Creates card registration object.</summary>
        /// <returns>CardPreAuthorization instance returned from API.</returns>
        static CardPreAuthorizationDTO GetJohnsCardPreAuthorization(string idempotencyKey)
        {
            UserNaturalDTO user = GetJohn();
            CardPreAuthorizationPostDTO cardPreAuthorization = getPreAuthorization(user.Id);

            return _cardPreAuthorizationsApi.Create(idempotencyKey, cardPreAuthorization).Result;
        }

        static CardPreAuthorizationPostDTO getPreAuthorization(string userId)
        {
            CardRegistrationPostDTO cardRegistrationPost = new CardRegistrationPostDTO(userId, CurrencyIso.EUR);
            CardRegistrationDTO newCardRegistration = _cardRegistrationsApi.Create(cardRegistrationPost).Result;

            CardRegistrationPutDTO cardRegistrationPut = new CardRegistrationPutDTO();
            string registrationData = GetPaylineCorrectRegistartionData(newCardRegistration);
            cardRegistrationPut.RegistrationData = registrationData;
            CardRegistrationDTO getCardRegistration = _cardRegistrationsApi.Update(cardRegistrationPut, newCardRegistration.Id).Result;

            CardPreAuthorizationPostDTO cardPreAuthorization = new CardPreAuthorizationPostDTO(userId, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, SecureMode.DEFAULT, getCardRegistration.CardId, "http://test.com");

            return cardPreAuthorization;

        }

        public static KycDocumentDTO GetJohnsKycDocument()
        {
            if (_johnsKycDocument == null)
            {
                string johnsId = GetJohn().Id;

                _johnsKycDocument = _usersApi.CreateKycDocument(johnsId, KycDocumentType.IDENTITY_PROOF).Result;
            }

            return _johnsKycDocument;
        }

        public static KycDocumentDTO GetNewKycDocument()
        {
            _johnsKycDocument = null;
            return GetJohnsKycDocument();
        }

        /// <summary>Gets registration data from Payline service.</summary>
        /// <param name="cardRegistration">CardRegistration instance.</param>
        /// <returns>Registration data.</returns>
        static string GetPaylineCorrectRegistartionData(CardRegistrationDTO cardRegistration)
        {
            RestClient client = new RestClient(cardRegistration.CardRegistrationURL);

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("data", cardRegistration.PreregistrationData);
            request.AddParameter("accessKeyRef", cardRegistration.AccessKey);
            request.AddParameter("cardNumber", "4970100000000154");
            request.AddParameter("cardExpirationDate", "1226");
            request.AddParameter("cardCvx", "123");

            // Payline requires TLS
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            IRestResponse response = client.Execute(request);

            string responseString = response.Content;

            if (response.StatusCode == HttpStatusCode.OK)
                return responseString;
            else
                throw new Exception(responseString);
        }

        static HookDTO GetJohnsHook()
        {
            if (_johnsHook == null)
            {

                Pagination pagination = new Pagination(1, 1);
                ListPaginated<HookDTO> list = _hooksApi.GetAll(pagination).Result;

                if (list != null && list.Count > 0 && list[0] != null)
                {
                    _johnsHook = list[0];
                }
                else
                {
                    HookPostDTO hook = new HookPostDTO("http://test.com", EventType.PAYIN_NORMAL_CREATED);
                    _johnsHook = _hooksApi.Create(hook).Result;
                }
            }

            return _johnsHook;
        }

        static ReportRequestDTO GetJohnsReport(ReportType reportType)
        {
            if (!_johnsReports.ContainsKey(reportType))
            {
                ReportRequestPostDTO reportPost = new ReportRequestPostDTO(ReportType.TRANSACTIONS);
                _johnsReports.Add(reportType, _reportsApi.Create(reportPost).Result);
            }

            return _johnsReports[reportType];
        }

        public static void AssertEqualInputProps<T>(T entity1, T entity2)
        {
            Assert.NotNull(entity1);
            Assert.NotNull(entity2);

            if (entity1 is UserNaturalDTO && entity2 is UserNaturalDTO)
            {
                Assert.Equal((entity1 as UserNaturalDTO).Tag, (entity2 as UserNaturalDTO).Tag);
                Assert.Equal((entity1 as UserNaturalDTO).PersonType, (entity2 as UserNaturalDTO).PersonType);
                Assert.Equal((entity1 as UserNaturalDTO).FirstName, (entity2 as UserNaturalDTO).FirstName);
                Assert.Equal((entity1 as UserNaturalDTO).LastName, (entity2 as UserNaturalDTO).LastName);
                Assert.Equal((entity1 as UserNaturalDTO).Email, (entity2 as UserNaturalDTO).Email);
                Assert.Equal((entity1 as UserNaturalDTO).Address.AddressLine1, (entity2 as UserNaturalDTO).Address.AddressLine1);
                Assert.Equal((entity1 as UserNaturalDTO).Address.AddressLine2, (entity2 as UserNaturalDTO).Address.AddressLine2);
                Assert.Equal((entity1 as UserNaturalDTO).Address.City, (entity2 as UserNaturalDTO).Address.City);
                Assert.Equal((entity1 as UserNaturalDTO).Address.Country, (entity2 as UserNaturalDTO).Address.Country);
                Assert.Equal((entity1 as UserNaturalDTO).Address.PostalCode, (entity2 as UserNaturalDTO).Address.PostalCode);
                Assert.Equal((entity1 as UserNaturalDTO).Address.Region, (entity2 as UserNaturalDTO).Address.Region);
                Assert.Equal((entity1 as UserNaturalDTO).Birthday, (entity2 as UserNaturalDTO).Birthday);
                Assert.Equal((entity1 as UserNaturalDTO).Nationality, (entity2 as UserNaturalDTO).Nationality);
                Assert.Equal((entity1 as UserNaturalDTO).CountryOfResidence, (entity2 as UserNaturalDTO).CountryOfResidence);
                Assert.Equal((entity1 as UserNaturalDTO).Occupation, (entity2 as UserNaturalDTO).Occupation);
                Assert.Equal((entity1 as UserNaturalDTO).IncomeRange, (entity2 as UserNaturalDTO).IncomeRange);
            }
            else if (entity1 is UserLegalDTO && entity2 is UserLegalDTO)
            {
                Assert.Equal((entity1 as UserLegalDTO).Tag, (entity2 as UserLegalDTO).Tag);
                Assert.Equal((entity1 as UserLegalDTO).PersonType, (entity2 as UserLegalDTO).PersonType);
                Assert.Equal((entity1 as UserLegalDTO).Name, (entity2 as UserLegalDTO).Name);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.AddressLine1, (entity2 as UserLegalDTO).HeadquartersAddress.AddressLine1);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.AddressLine2, (entity2 as UserLegalDTO).HeadquartersAddress.AddressLine2);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.City, (entity2 as UserLegalDTO).HeadquartersAddress.City);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.Country, (entity2 as UserLegalDTO).HeadquartersAddress.Country);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.PostalCode, (entity2 as UserLegalDTO).HeadquartersAddress.PostalCode);
                Assert.Equal((entity1 as UserLegalDTO).HeadquartersAddress.Region, (entity2 as UserLegalDTO).HeadquartersAddress.Region);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeFirstName, (entity2 as UserLegalDTO).LegalRepresentativeFirstName);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeLastName, (entity2 as UserLegalDTO).LegalRepresentativeLastName);
                //Assert.AreEqual("***** TEMPORARY API ISSUE: RETURNED OBJECT MISSES THIS PROP AFTER CREATION *****", (entity1 as UserLegal).LegalRepresentativeAddress, (entity2 as UserLegal).LegalRepresentativeAddress);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeEmail, (entity2 as UserLegalDTO).LegalRepresentativeEmail);
                //Assert.AreEqual("***** TEMPORARY API ISSUE: RETURNED OBJECT HAS THIS PROP CHANGED FROM TIMESTAMP INTO ISO string AFTER CREATION *****", (entity1 as UserLegal).LegalRepresentativeBirthday, (entity2 as UserLegal).LegalRepresentativeBirthday);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeBirthday, (entity2 as UserLegalDTO).LegalRepresentativeBirthday);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeNationality, (entity2 as UserLegalDTO).LegalRepresentativeNationality);
                Assert.Equal((entity1 as UserLegalDTO).LegalRepresentativeCountryOfResidence, (entity2 as UserLegalDTO).LegalRepresentativeCountryOfResidence);
            }
            else if (entity1 is BankAccountDTO && entity2 is BankAccountDTO)
            {
                Assert.Equal((entity1 as BankAccountDTO).Tag, (entity2 as BankAccountDTO).Tag);
                Assert.Equal((entity1 as BankAccountDTO).UserId, (entity2 as BankAccountDTO).UserId);
                Assert.Equal((entity1 as BankAccountDTO).Type, (entity2 as BankAccountDTO).Type);
                Assert.Equal((entity1 as BankAccountDTO).OwnerName, (entity2 as BankAccountDTO).OwnerName);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.AddressLine1, (entity2 as BankAccountDTO).OwnerAddress.AddressLine1);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.AddressLine2, (entity2 as BankAccountDTO).OwnerAddress.AddressLine2);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.City, (entity2 as BankAccountDTO).OwnerAddress.City);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.Country, (entity2 as BankAccountDTO).OwnerAddress.Country);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.PostalCode, (entity2 as BankAccountDTO).OwnerAddress.PostalCode);
                Assert.Equal((entity1 as BankAccountDTO).OwnerAddress.Region, (entity2 as BankAccountDTO).OwnerAddress.Region);
                if ((entity1 as BankAccountDTO).Type == BankAccountType.IBAN)
                {
                    Assert.Equal((entity1 as BankAccountIbanDTO).IBAN, (entity2 as BankAccountIbanDTO).IBAN);
                    Assert.Equal((entity1 as BankAccountIbanDTO).BIC, (entity2 as BankAccountIbanDTO).BIC);
                }
                else if ((entity1 as BankAccountDTO).Type == BankAccountType.GB)
                {
                    Assert.Equal((entity1 as BankAccountGbDTO).AccountNumber, (entity2 as BankAccountGbDTO).AccountNumber);
                    Assert.Equal((entity1 as BankAccountGbDTO).SortCode, (entity2 as BankAccountGbDTO).SortCode);
                }
                else if ((entity1 as BankAccountDTO).Type == BankAccountType.US)
                {
                    Assert.Equal((entity1 as BankAccountUsDTO).AccountNumber, (entity2 as BankAccountUsDTO).AccountNumber);
                    Assert.Equal((entity1 as BankAccountUsDTO).ABA, (entity2 as BankAccountUsDTO).ABA);
                }
                else if ((entity1 as BankAccountDTO).Type == BankAccountType.CA)
                {
                    Assert.Equal((entity1 as BankAccountCaDTO).AccountNumber, (entity2 as BankAccountCaDTO).AccountNumber);
                    Assert.Equal((entity1 as BankAccountCaDTO).BankName, (entity2 as BankAccountCaDTO).BankName);
                    Assert.Equal((entity1 as BankAccountCaDTO).InstitutionNumber, (entity2 as BankAccountCaDTO).InstitutionNumber);
                    Assert.Equal((entity1 as BankAccountCaDTO).BranchCode, (entity2 as BankAccountCaDTO).BranchCode);
                }
                else if ((entity1 as BankAccountDTO).Type == BankAccountType.OTHER)
                {
                    Assert.Equal((entity1 as BankAccountOtherDTO).AccountNumber, (entity2 as BankAccountOtherDTO).AccountNumber);
                    Assert.Equal((entity1 as BankAccountOtherDTO).Type, (entity2 as BankAccountOtherDTO).Type);
                    Assert.Equal((entity1 as BankAccountOtherDTO).Country, (entity2 as BankAccountOtherDTO).Country);
                    Assert.Equal((entity1 as BankAccountOtherDTO).BIC, (entity2 as BankAccountOtherDTO).BIC);
                }
            }
            else if (entity1 is PayInDTO && entity2 is PayInDTO)
            {
                Assert.Equal((entity1 as PayInDTO).Tag, (entity2 as PayInDTO).Tag);
                Assert.Equal((entity1 as PayInDTO).AuthorId, (entity2 as PayInDTO).AuthorId);
                Assert.Equal((entity1 as PayInDTO).CreditedUserId, (entity2 as PayInDTO).CreditedUserId);

                AssertEqualInputProps((entity1 as PayInDTO).DebitedFunds, (entity2 as PayInDTO).DebitedFunds);
                AssertEqualInputProps((entity1 as PayInDTO).CreditedFunds, (entity2 as PayInDTO).CreditedFunds);
                AssertEqualInputProps((entity1 as PayInDTO).Fees, (entity2 as PayInDTO).Fees);
            }
            else if (typeof(T) == typeof(CardDTO))
            {
                Assert.Equal((entity1 as CardDTO).ExpirationDate, (entity2 as CardDTO).ExpirationDate);
                Assert.Equal((entity1 as CardDTO).Alias, (entity2 as CardDTO).Alias);
                Assert.Equal((entity1 as CardDTO).CardType, (entity2 as CardDTO).CardType);
                Assert.Equal((entity1 as CardDTO).Currency, (entity2 as CardDTO).Currency);
            }
            else if (typeof(T) == typeof(PayOutDTO))
            {
                Assert.Equal((entity1 as PayOutDTO).Tag, (entity2 as PayOutDTO).Tag);
                Assert.Equal((entity1 as PayOutDTO).AuthorId, (entity2 as PayOutDTO).AuthorId);
                Assert.Equal((entity1 as PayOutDTO).CreditedUserId, (entity2 as PayOutDTO).CreditedUserId);

                AssertEqualInputProps((entity1 as PayOutDTO).DebitedFunds, (entity2 as PayOutDTO).DebitedFunds);
                AssertEqualInputProps((entity1 as PayOutDTO).CreditedFunds, (entity2 as PayOutDTO).CreditedFunds);
                AssertEqualInputProps((entity1 as PayOutDTO).Fees, (entity2 as PayOutDTO).Fees);
            }
            else if (typeof(T) == typeof(TransferDTO))
            {
                Assert.Equal((entity1 as TransferDTO).Tag, (entity2 as TransferDTO).Tag);
                Assert.Equal((entity1 as TransferDTO).AuthorId, (entity2 as TransferDTO).AuthorId);
                Assert.Equal((entity1 as TransferDTO).CreditedUserId, (entity2 as TransferDTO).CreditedUserId);

                AssertEqualInputProps((entity1 as TransferDTO).DebitedFunds, (entity2 as TransferDTO).DebitedFunds);
                AssertEqualInputProps((entity1 as TransferDTO).CreditedFunds, (entity2 as TransferDTO).CreditedFunds);
                AssertEqualInputProps((entity1 as TransferDTO).Fees, (entity2 as TransferDTO).Fees);
            }
            else if (typeof(T) == typeof(TransactionDTO))
            {
                Assert.Equal((entity1 as TransactionDTO).Tag, (entity2 as TransactionDTO).Tag);

                AssertEqualInputProps((entity1 as TransactionDTO).DebitedFunds, (entity2 as TransactionDTO).DebitedFunds);
                AssertEqualInputProps((entity1 as TransactionDTO).Fees, (entity2 as TransactionDTO).Fees);
                AssertEqualInputProps((entity1 as TransactionDTO).CreditedFunds, (entity2 as TransactionDTO).CreditedFunds);

                Assert.Equal((entity1 as TransactionDTO).Status, (entity2 as TransactionDTO).Status);
            }
            else if (typeof(T) == typeof(Money))
            {
                Assert.Equal((entity1 as Money).Currency, (entity2 as Money).Currency);
                Assert.Equal((entity1 as Money).Amount, (entity2 as Money).Amount);
            }
            else
            {
                throw new ArgumentException("Unsupported type.");
            }
        }

        static MandateDTO GetNewMandate()
        {
            string bankAccountId = GetJohnsAccount().Id;
            string returnUrl = "http://test.test";
            MandatePostDTO mandatePost = new MandatePostDTO(bankAccountId, CultureCode.EN, returnUrl);
            MandateDTO mandate = _mandatesApi.Create(mandatePost).Result;

            return mandate;
        }
    }
}
