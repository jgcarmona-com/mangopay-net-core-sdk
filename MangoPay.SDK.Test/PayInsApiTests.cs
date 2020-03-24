using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.PayIns;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class PayInsApiTests : TestBase
    {
        private PayInsApi _objectToTest;
        private Mock<ILogger<PayInsApi>> _loggerMock;

        public PayInsApiTests()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IPayInsApi>() as PayInsApi;
        }

        [Fact]
        public void UserServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<PayInsApi>>();
            var authServiceMock = new Mock<IAuthApi>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new PayInsApi(config, _loggerMock.Object, authServiceMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }
        [Fact]
        public void Test_PayIns_Get_CardWeb()
        {
            try
            {
                PayInDTO payIn = null;
                payIn = TestHelper.GetJohnsPayInCardWeb();

                PayInDTO getPayIn = _objectToTest.Get(payIn.Id).Result;

                Assert.True(payIn.Id == getPayIn.Id);
                Assert.True(payIn.PaymentType == PayInPaymentType.CARD);
                Assert.True(payIn.ExecutionType == PayInExecutionType.WEB);

                TestHelper.AssertEqualInputProps(payIn, getPayIn);

                Assert.True(getPayIn.Status == TransactionStatus.CREATED);
                Assert.Null(getPayIn.ExecutionDate);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Create_PayPal()
        {
            try
            {
                PayInDTO payIn = null;
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();

                PayInPayPalPostDTO payInPost = new PayInPayPalPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://test/test");

                payIn = _objectToTest.CreatePayPal(payInPost).Result;

                Assert.True(payIn.Id.Length > 0);
                Assert.True(payIn.PaymentType == PayInPaymentType.PAYPAL);
                Assert.True(payIn.ExecutionType == PayInExecutionType.WEB);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Create_PayPal_WithShippingAddress()
        {
            try
            {
                PayInPayPalDTO payIn = null;
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();
                Address AddressForShippingAddress = new Address
                {
                    AddressLine1 = "Address line 1",
                    AddressLine2 = "Address line 2",
                    City = "City",
                    Country = CountryIso.PL,
                    PostalCode = "11222",
                    Region = "Region"
                };
                PayInPayPalPostDTO payInPost = new PayInPayPalPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://test/test");
                payInPost.ShippingAddress = new ShippingAddress("recipient name", AddressForShippingAddress);

                payIn = _objectToTest.CreatePayPal(payInPost).Result;

                Assert.NotNull(payIn.ShippingAddress);
                Assert.Equal("recipient name", payIn.ShippingAddress.RecipientName);
                Assert.NotNull(payIn.ShippingAddress.Address);
                Assert.Equal("Address line 1", payIn.ShippingAddress.Address.AddressLine1);
                Assert.Equal("Address line 2", payIn.ShippingAddress.Address.AddressLine2);
                Assert.Equal("City", payIn.ShippingAddress.Address.City);
                Assert.Equal(CountryIso.PL, payIn.ShippingAddress.Address.Country);
                Assert.Equal("11222", payIn.ShippingAddress.Address.PostalCode);
                Assert.Equal("Region", payIn.ShippingAddress.Address.Region);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Create_CardDirect()
        {
            try
            {
                WalletDTO johnWallet = TestHelper.GetJohnsWalletWithMoney();
                WalletDTO beforeWallet = TestHelper.WalletsApi.Get(johnWallet.Id).Result;

                PayInDTO payIn = TestHelper.GetNewPayInCardDirect();
                WalletDTO wallet = TestHelper.WalletsApi.Get(johnWallet.Id).Result;
                UserNaturalDTO user = TestHelper.GetJohn();

                Assert.True(payIn.Id.Length > 0);
                Assert.Equal(wallet.Id, payIn.CreditedWalletId);
                Assert.Equal(PayInPaymentType.CARD, payIn.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, payIn.ExecutionType);
                Assert.True(payIn.DebitedFunds is Money);
                Assert.True(payIn.CreditedFunds is Money);
                Assert.True(payIn.Fees is Money);
                Assert.Equal(user.Id, payIn.AuthorId);
                Assert.True(wallet.Balance.Amount == beforeWallet.Balance.Amount + payIn.CreditedFunds.Amount);
                Assert.Equal(TransactionStatus.SUCCEEDED, payIn.Status);
                Assert.Equal(TransactionType.PAYIN, payIn.Type);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Payins_CardDirect_Create_WithBilling()
        {
            try
            {
                WalletDTO johnWallet = TestHelper.GetJohnsWalletWithMoney();
                WalletDTO wallet = TestHelper.WalletsApi.Get(johnWallet.Id).Result;
                UserNaturalDTO user = TestHelper.GetJohn();

                PayInCardDirectDTO payIn = TestHelper.GetNewPayInCardDirectWithBilling();

                Assert.True(payIn.Id.Length > 0);
                Assert.Equal(wallet.Id, payIn.CreditedWalletId);
                Assert.Equal(PayInPaymentType.CARD, payIn.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, payIn.ExecutionType);
                Assert.True(payIn.DebitedFunds is Money);
                Assert.True(payIn.CreditedFunds is Money);
                Assert.True(payIn.Fees is Money);
                Assert.Equal(user.Id, payIn.AuthorId);
                Assert.Equal(TransactionStatus.SUCCEEDED, payIn.Status);
                Assert.Equal(TransactionType.PAYIN, payIn.Type);
                Assert.NotNull(payIn.Billing);
                Assert.NotNull(payIn.SecurityInfo);
                Assert.Equal(AVSResult.ADDRESS_MATCH_ONLY, payIn.SecurityInfo.AVSResult);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Get_CardDirect()
        {
            try
            {
                PayInCardDirectDTO payIn = TestHelper.GetNewPayInCardDirect();

                PayInCardDirectDTO getPayIn = _objectToTest.GetCardDirect(payIn.Id).Result;

                Assert.True(payIn.Id == getPayIn.Id);
                Assert.True(payIn.PaymentType == PayInPaymentType.CARD);
                Assert.True(payIn.ExecutionType == PayInExecutionType.DIRECT);
                TestHelper.AssertEqualInputProps(payIn, getPayIn);
                Assert.NotNull(getPayIn.CardId);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_CreateRefund_CardDirect()
        {
            try
            {
                PayInDTO payIn = TestHelper.GetNewPayInCardDirect();
                WalletDTO wallet = TestHelper.GetJohnsWalletWithMoney();
                WalletDTO walletBefore = TestHelper.WalletsApi.Get(wallet.Id).Result;

                RefundDTO refund = TestHelper.GetNewRefundForPayIn(payIn);
                WalletDTO walletAfter = TestHelper.WalletsApi.Get(wallet.Id).Result;

                Assert.True(refund.Id.Length > 0);
                Assert.True(refund.DebitedFunds.Amount == payIn.DebitedFunds.Amount);
                Assert.True(walletBefore.Balance.Amount == (walletAfter.Balance.Amount + payIn.DebitedFunds.Amount));
                Assert.Equal(TransactionType.PAYOUT, refund.Type);
                Assert.Equal(TransactionNature.REFUND, refund.Nature);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_PreAuthorizedDirect()
        {
            try
            {
                CardPreAuthorizationDTO cardPreAuthorization = TestHelper.GetJohnsCardPreAuthorization();
                WalletDTO wallet = TestHelper.GetJohnsWalletWithMoney();
                UserNaturalDTO user = TestHelper.GetJohn();

                // create pay-in PRE-AUTHORIZED DIRECT
                PayInPreauthorizedDirectPostDTO payIn = new PayInPreauthorizedDirectPostDTO(user.Id, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, cardPreAuthorization.Id);

                payIn.SecureModeReturnURL = "http://test.com";

                PayInPreauthorizedDirectDTO createPayIn = _objectToTest.CreatePreauthorizedDirect(payIn).Result;

                Assert.True("" != createPayIn.Id);
                Assert.Equal(wallet.Id, createPayIn.CreditedWalletId);
                Assert.Equal(PayInPaymentType.PREAUTHORIZED, createPayIn.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, createPayIn.ExecutionType);
                Assert.True(createPayIn.DebitedFunds is Money);
                Assert.True(createPayIn.CreditedFunds is Money);
                Assert.True(createPayIn.Fees is Money);
                Assert.Equal(user.Id, createPayIn.AuthorId);
                Assert.Equal(TransactionStatus.SUCCEEDED, createPayIn.Status);
                Assert.Equal(TransactionType.PAYIN, createPayIn.Type);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_BankWireDirect_Create()
        {
            try
            {
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();

                // create pay-in BANKWIRE DIRECT
                PayInBankWireDirectPostDTO payIn = new PayInBankWireDirectPostDTO(user.Id, wallet.Id, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR });
                payIn.CreditedWalletId = wallet.Id;
                payIn.AuthorId = user.Id;

                PayInDTO createPayIn = _objectToTest.CreateBankWireDirect(payIn).Result;

                Assert.True(createPayIn.Id.Length > 0);
                Assert.Equal(wallet.Id, createPayIn.CreditedWalletId);
                Assert.Equal(PayInPaymentType.BANK_WIRE, createPayIn.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, createPayIn.ExecutionType);
                Assert.Equal(user.Id, createPayIn.AuthorId);
                Assert.Equal(TransactionStatus.CREATED, createPayIn.Status);
                Assert.Equal(TransactionType.PAYIN, createPayIn.Type);
                Assert.NotNull(((PayInBankWireDirectDTO)createPayIn).WireReference);
                Assert.Equal(BankAccountType.IBAN, ((PayInBankWireDirectDTO)createPayIn).BankAccount.Type);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        /*
		 * Uncomment the attribute below to test payins with a mandate
		 * This test needs your manual confirmation on the web page (see note in test's body)
		 */
        //[Fact]
     ////   public void Test_PayIns_MandateDirect_Create_Get()
     ////   {
     ////       try
     ////       {
     ////           WalletDTO wallet = TestHelper.GetJohnsWallet();
     ////           UserNaturalDTO user = TestHelper.GetJohn();

     ////           string bankAccountId = TestHelper.GetJohnsAccount().Id;
     ////           string returnUrl = "http://test.test";
     ////           MandatePostDTO mandatePost = new MandatePostDTO(bankAccountId, CultureCode.EN, returnUrl);
     ////           MandateDTO mandate = this.Api.Mandates.Create(mandatePost);

     ////           /*	
				 ////*	! IMPORTANT NOTE !
				 ////*	
				 ////*	In order to make this test pass, at this place you have to set a breakpoint,
				 ////*	navigate to URL the mandate.RedirectURL property points to and click "CONFIRM" button.
				 ////* 
				 ////*/

     ////           PayInMandateDirectPostDTO payIn = new PayInMandateDirectPostDTO(user.Id, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://test.test", mandate.Id);

     ////           PayInDTO createPayIn = _objectToTest.CreateMandateDirectDebit(payIn).Result;

     ////           Assert.NotNull(createPayIn);
     ////           Assert.NotEqual(TransactionStatus.FAILED, createPayIn.Status);
     ////           //, "In order to make this test pass, after creating mandate and before creating the payin you have to navigate to URL the mandate.RedirectURL property points to and click CONFIRM button.");

     ////           Assert.True(createPayIn.Id.Length > 0);
     ////           Assert.Equal(wallet.Id, createPayIn.CreditedWalletId);
     ////           Assert.Equal(PayInPaymentType.DIRECT_DEBIT, createPayIn.PaymentType);
     ////           Assert.Equal(PayInExecutionType.DIRECT, createPayIn.ExecutionType);
     ////           Assert.Equal(user.Id, createPayIn.AuthorId);
     ////           Assert.Equal(TransactionStatus.CREATED, createPayIn.Status);
     ////           Assert.Equal(TransactionType.PAYIN, createPayIn.Type);
     ////           Assert.NotNull(((PayInMandateDirectDTO)createPayIn).MandateId);
     ////           Assert.Equal(((PayInMandateDirectDTO)createPayIn).MandateId, mandate.Id);

     ////           PayInMandateDirectDTO getPayIn = _objectToTest.GetMandateDirectDebit(createPayIn.Id).Result;

     ////           Assert.NotNull(getPayIn);
     ////           Assert.True(getPayIn.Id == createPayIn.Id);
     ////       }
     ////       catch (Exception ex)
     ////       {
     ////           Assert.True(false, ex.Message);
     ////       }
     ////   }

        [Fact]
        public void Test_PayIns_BankWireDirect_Get()
        {
            try
            {
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();

                // create pay-in BANKWIRE DIRECT
                PayInBankWireDirectPostDTO payIn = new PayInBankWireDirectPostDTO(user.Id, wallet.Id, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR });
                payIn.CreditedWalletId = wallet.Id;
                payIn.AuthorId = user.Id;

                PayInBankWireDirectDTO createdPayIn = _objectToTest.CreateBankWireDirect(payIn).Result;
                System.Threading.Thread.Sleep(1000);
                PayInBankWireDirectDTO getPayIn = _objectToTest.GetBankWireDirect(createdPayIn.Id).Result;

                Assert.Equal(getPayIn.Id, createdPayIn.Id);
                Assert.Equal(PayInPaymentType.BANK_WIRE, getPayIn.PaymentType);
                Assert.Equal(PayInExecutionType.DIRECT, getPayIn.ExecutionType);
                Assert.Equal(user.Id, getPayIn.AuthorId);
                Assert.Equal(TransactionType.PAYIN, getPayIn.Type);
                Assert.NotNull(getPayIn.WireReference);
                Assert.Equal(BankAccountType.IBAN, getPayIn.BankAccount.Type);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_DirectDebit_Create_Get()
        {
            WalletDTO wallet = TestHelper.GetJohnsWallet();
            UserNaturalDTO user = TestHelper.GetJohn();
            // create pay-in DIRECT DEBIT
            PayInDirectDebitPostDTO payIn = new PayInDirectDebitPostDTO(user.Id, new Money { Amount = 10000, Currency = CurrencyIso.EUR }, new Money { Amount = 100, Currency = CurrencyIso.EUR }, wallet.Id, "http://www.mysite.com/returnURL/", CultureCode.FR, DirectDebitType.GIROPAY);

            payIn.TemplateURLOptions = new TemplateURLOptions { PAYLINE = "https://www.maysite.com/payline_template/" };
            payIn.Tag = "DirectDebit test tag";

            PayInDirectDebitDTO createPayIn = _objectToTest.CreateDirectDebit(payIn).Result;

            Assert.NotNull(createPayIn);
            Assert.True(createPayIn.Id.Length > 0);
            Assert.Equal(wallet.Id, createPayIn.CreditedWalletId);
            Assert.True(createPayIn.PaymentType == PayInPaymentType.DIRECT_DEBIT);
            Assert.True(createPayIn.DirectDebitType == DirectDebitType.GIROPAY);
            Assert.True(createPayIn.Culture == CultureCode.FR);
            Assert.Equal(user.Id, createPayIn.AuthorId);
            Assert.True(createPayIn.Status == TransactionStatus.CREATED);
            Assert.True(createPayIn.Type == TransactionType.PAYIN);
            Assert.NotNull(createPayIn.DebitedFunds);
            Assert.True(createPayIn.DebitedFunds is Money);
            Assert.Equal(10000, createPayIn.DebitedFunds.Amount);
            Assert.True(createPayIn.DebitedFunds.Currency == CurrencyIso.EUR);

            Assert.NotNull(createPayIn.CreditedFunds);
            Assert.True(createPayIn.CreditedFunds is Money);
            Assert.Equal(9900, createPayIn.CreditedFunds.Amount);
            Assert.True(createPayIn.CreditedFunds.Currency == CurrencyIso.EUR);

            Assert.NotNull(createPayIn.Fees);
            Assert.True(createPayIn.Fees is Money);
            Assert.Equal(100, createPayIn.Fees.Amount);
            Assert.True(createPayIn.Fees.Currency == CurrencyIso.EUR);

            Assert.NotNull(createPayIn.ReturnURL);
            Assert.NotNull(createPayIn.RedirectURL);
            Assert.NotNull(createPayIn.TemplateURL);


            PayInDirectDebitDTO getPayIn = _objectToTest.GetDirectDebit(createPayIn.Id).Result;

            Assert.NotNull(getPayIn);
            Assert.True(getPayIn.Id == createPayIn.Id);
            Assert.True(getPayIn.Tag == createPayIn.Tag);
        }

        [Fact]
        public void Test_PayIns_Get_PayPal()
        {
            try
            {
                PayInDTO payIn = null;
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();

                PayInPayPalPostDTO payInPost = new PayInPayPalPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://test/test");

                payIn = _objectToTest.CreatePayPal(payInPost).Result;

                Assert.True(payIn.Id.Length > 0);
                Assert.True(payIn.PaymentType == PayInPaymentType.PAYPAL);
                Assert.True(payIn.ExecutionType == PayInExecutionType.WEB);

                PayInPayPalDTO getPayIn = _objectToTest.GetPayPal(payIn.Id).Result;

                Assert.NotNull(getPayIn);
                Assert.True(getPayIn.Id == payIn.Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Get_PayPal_WithShippingAddress()
        {
            try
            {
                PayInDTO payIn = null;
                WalletDTO wallet = TestHelper.GetJohnsWallet();
                UserNaturalDTO user = TestHelper.GetJohn();
                Address AddressForShippingAddress = new Address
                {
                    AddressLine1 = "Address line 1",
                    AddressLine2 = "Address line 2",
                    City = "City",
                    Country = CountryIso.PL,
                    PostalCode = "11222",
                    Region = "Region"
                };
                PayInPayPalPostDTO payInPost = new PayInPayPalPostDTO(user.Id, new Money { Amount = 1000, Currency = CurrencyIso.EUR }, new Money { Amount = 0, Currency = CurrencyIso.EUR }, wallet.Id, "http://test/test");
                payInPost.ShippingAddress = new ShippingAddress("recipient name", AddressForShippingAddress);
                payIn = _objectToTest.CreatePayPal(payInPost).Result;

                PayInPayPalDTO getPayIn = _objectToTest.GetPayPal(payIn.Id).Result;

                Assert.NotNull(getPayIn.ShippingAddress);
                Assert.Equal("recipient name", getPayIn.ShippingAddress.RecipientName);
                Assert.NotNull(getPayIn.ShippingAddress.Address);
                Assert.Equal("Address line 1", getPayIn.ShippingAddress.Address.AddressLine1);
                Assert.Equal("Address line 2", getPayIn.ShippingAddress.Address.AddressLine2);
                Assert.Equal("City", getPayIn.ShippingAddress.Address.City);
                Assert.Equal(CountryIso.PL, getPayIn.ShippingAddress.Address.Country);
                Assert.Equal("11222", getPayIn.ShippingAddress.Address.PostalCode);
                Assert.Equal("Region", getPayIn.ShippingAddress.Address.Region);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_PayIns_Get_PayPal_WithPayPalBuyerAccountEmail()
        {
            try
            {
                string payInId = "54088959";
                string payPalBuyerEmail = "paypal-buyer-user@mangopay.com";
                PayInPayPalDTO payIn = _objectToTest.GetPayPal(payInId).Result;

                Assert.NotNull(payIn);
                Assert.NotNull(payIn.Id);
                Assert.NotNull(payIn.PaypalBuyerAccountEmail);
                Assert.Equal(payInId, payIn.Id);
                Assert.Equal(payPalBuyerEmail, payIn.PaypalBuyerAccountEmail);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
