using MangoPay.SDK.APIs.Clients;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class ApiClientsTest : TestBase
    {
        private ClientsApi _objectToTest;
        public ApiClientsTest()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IClientsApi>() as ClientsApi;
        }

        [Fact]
        public void Test_Client_GetKycDocuments()
        {
            ListPaginated<KycDocumentDTO> result = null;
            ListPaginated<KycDocumentDTO> result2 = null;

            result = _objectToTest.GetKycDocuments(null, null).Result;
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            Pagination pagination = new Pagination(1, 2);
            Sort sort = new Sort();
            sort.AddField("CreationDate", SortDirection.asc);
            result = _objectToTest.GetKycDocuments(pagination, null, sort).Result;
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            sort = new Sort();
            sort.AddField("CreationDate", SortDirection.desc);
            result2 = _objectToTest.GetKycDocuments(pagination, null, sort).Result;
            Assert.NotNull(result2);
            Assert.True(result2.Count > 0);

            Assert.True(result[0].Id != result2[0].Id);
        }

        [Fact]
        public void Test_Client_GetWallets()
        {
            ListPaginated<WalletDTO> feesWallets = null;
            ListPaginated<WalletDTO> creditWallets = null;

            feesWallets = _objectToTest.GetWallets(FundsType.FEES, new Pagination(1, 100)).Result;
            creditWallets = _objectToTest.GetWallets(FundsType.CREDIT, new Pagination(1, 100)).Result;

            Assert.NotNull(feesWallets);
            Assert.NotNull(creditWallets);
        }

        [Fact]
        public void Test_Client_GetWallet()
        {
            ListPaginated<WalletDTO> feesWallets = null;
            ListPaginated<WalletDTO> creditWallets = null;
            try
            {
                feesWallets = _objectToTest.GetWallets(FundsType.FEES, new Pagination(1, 1)).Result;
                creditWallets = _objectToTest.GetWallets(FundsType.CREDIT, new Pagination(1, 1)).Result;
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

            if ((feesWallets == null || feesWallets.Count == 0) ||
                (creditWallets == null || creditWallets.Count == 0))
                Assert.True(false, "Cannot test getting client's wallet because there is no any wallet for client.");

            WalletDTO wallet = null;
            WalletDTO result = null;
            if (feesWallets != null && feesWallets.Count > 0)
                wallet = feesWallets[0];
            else if (creditWallets != null && creditWallets.Count > 0)
                wallet = creditWallets[0];

            result = _objectToTest.GetWallet(wallet.FundsType, wallet.Currency).Result;

            Assert.NotNull(result);
            Assert.True(result.FundsType == wallet.FundsType);
            Assert.True(result.Currency == wallet.Currency);
        }

        [Fact]
        public void Test_Client_GetWalletTransactions()
        {
            ListPaginated<WalletDTO> feesWallets = null;
            ListPaginated<WalletDTO> creditWallets = null;
            try
            {
                feesWallets = _objectToTest.GetWallets(FundsType.FEES, new Pagination(1, 1)).Result;
                creditWallets = _objectToTest.GetWallets(FundsType.CREDIT, new Pagination(1, 1)).Result;
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

            if ((feesWallets == null || feesWallets.Count == 0) ||
                (creditWallets == null || creditWallets.Count == 0))
                Assert.True(false, "Cannot test getting client's wallet transactions because there is no any wallet for client.");

            WalletDTO wallet = null;
            ListPaginated<TransactionDTO> result = null;
            if (feesWallets != null && feesWallets.Count > 0)
                wallet = feesWallets[0];
            else if (creditWallets != null && creditWallets.Count > 0)
                wallet = creditWallets[0];

            result = _objectToTest.GetWalletTransactions(wallet.FundsType, wallet.Currency, new Pagination(1, 1), null).Result;

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Test_Client_GetTransactions()
        {
            ListPaginated<TransactionDTO> result = null;

            try
            {
                result = _objectToTest.GetTransactions(null, null).Result;
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

            Assert.NotNull(result);
        }

        [Fact]
        public void Test_Client_CreateBankWireDirect()
        {
            try
            {
                ClientBankWireDirectPostDTO bankwireDirectPost = new ClientBankWireDirectPostDTO("CREDIT_EUR", new Money { Amount = 1000, Currency = CurrencyIso.EUR });

                PayInDTO result = _objectToTest.CreateBankWireDirect(bankwireDirectPost).Result;

                Assert.True(result.Id.Length > 0);
                Assert.Equal("CREDIT_EUR", result.CreditedWalletId);
                Assert.Equal(PayInPaymentType.BANK_WIRE, result.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, result.ExecutionType);
                Assert.Equal(TransactionStatus.CREATED, result.Status);
                Assert.Equal(TransactionType.PAYIN, result.Type);
                Assert.NotNull(((PayInBankWireDirectDTO)result).WireReference);
                Assert.Equal(BankAccountType.IBAN, ((PayInBankWireDirectDTO)result).BankAccount.Type);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_ClientGet()
        {
            ClientDTO client = _objectToTest.Get().Result;

            Assert.NotNull(client);
            Assert.True("sdk-unit-tests".Equals(client.ClientId));
        }

        [Fact]
        public void Test_ClientSave()
        {
            ClientPutDTO client = new ClientPutDTO();

            Random rand = new Random();
            string color1 = (rand.Next(100000) + 100000).ToString();
            string color2 = (rand.Next(100000) + 100000).ToString();
            string headquartersPhoneNumber = (rand.Next(10000000, 99999999)).ToString();

            client.PrimaryButtonColour = "#" + color1;
            client.PrimaryThemeColour = "#" + color2;
            client.AdminEmails = new List<string> { "support@mangopay.com", "technical@mangopay.com" };
            client.BillingEmails = new List<string> { "support@mangopay.com", "technical@mangopay.com" };
            client.FraudEmails = new List<string> { "support@mangopay.com", "technical@mangopay.com" };
            client.TechEmails = new List<string> { "support@mangopay.com", "technical@mangopay.com" };
            client.TaxNumber = "123456";
            client.PlatformDescription = "Description";
            client.PlatformType = PlatformType.MARKETPLACE;
            client.PlatformURL = "http://test.com";
            client.HeadquartersAddress = new Address
            {
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                City = "City",
                Country = CountryIso.FR,
                PostalCode = "51234",
                Region = "Region"
            };
            client.HeadquartersPhoneNumber = headquartersPhoneNumber;

            ClientDTO clientNew = _objectToTest.Save(client).Result;

            Assert.NotNull(clientNew);
            Assert.Equal(client.PrimaryButtonColour, clientNew.PrimaryButtonColour);
            Assert.Equal(client.PrimaryThemeColour, clientNew.PrimaryThemeColour);
            Assert.Equal(2, client.AdminEmails.Count);
            Assert.Equal("support@mangopay.com", client.AdminEmails[0]);
            Assert.Equal("technical@mangopay.com", client.AdminEmails[1]);
            Assert.Equal(2, client.BillingEmails.Count);
            Assert.Equal("support@mangopay.com", client.BillingEmails[0]);
            Assert.Equal("technical@mangopay.com", client.BillingEmails[1]);
            Assert.Equal(2, client.FraudEmails.Count);
            Assert.Equal("support@mangopay.com", client.FraudEmails[0]);
            Assert.Equal("technical@mangopay.com", client.FraudEmails[1]);
            Assert.Equal(2, client.TechEmails.Count);
            Assert.Equal("support@mangopay.com", client.TechEmails[0]);
            Assert.Equal("technical@mangopay.com", client.TechEmails[1]);
            Assert.Equal("123456", client.TaxNumber);
            Assert.Equal("Description", client.PlatformDescription);
            Assert.Equal(PlatformType.MARKETPLACE, client.PlatformType);
            Assert.Equal("http://test.com", client.PlatformURL);
            Assert.NotNull(client.HeadquartersAddress);
            Assert.Equal("AddressLine1", client.HeadquartersAddress.AddressLine1);
            Assert.Equal("AddressLine2", client.HeadquartersAddress.AddressLine2);
            Assert.Equal("City", client.HeadquartersAddress.City);
            Assert.Equal(CountryIso.FR, client.HeadquartersAddress.Country);
            Assert.Equal("51234", client.HeadquartersAddress.PostalCode);
            Assert.Equal("Region", client.HeadquartersAddress.Region);
            Assert.NotNull(client.HeadquartersPhoneNumber);
            Assert.Equal(headquartersPhoneNumber, client.HeadquartersPhoneNumber);
        }

        [Fact]
        public void Test_Client_SaveAddressNull()
        {
            ClientPutDTO client = new ClientPutDTO();

            Random rand = new Random();
            string color1 = (rand.Next(100000) + 100000).ToString();
            string color2 = (rand.Next(100000) + 100000).ToString();

            client.PrimaryButtonColour = "#" + color1;
            client.PrimaryThemeColour = "#" + color2;
            client.HeadquartersAddress = new Address();

            ClientDTO clientNew = _objectToTest.Save(client).Result;

            Assert.NotNull(clientNew);
        }

        [Fact]
        public async Task Test_ClientLogo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
            FileInfo fi = assemblyFileInfo.Directory.GetFiles("TestKycPageFile.png").Single();

            await _objectToTest.UploadLogo(fi.FullName);
            await _objectToTest.UploadLogo(File.ReadAllBytes(fi.FullName));
        }
    }
}
