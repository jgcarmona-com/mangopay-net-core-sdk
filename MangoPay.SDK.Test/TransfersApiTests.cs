using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.Transfers;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class TransfersApiTests : TestBase
    {
        private TransfersApi _objectToTest;
        private Mock<ILogger<TransfersApi>> _loggerMock;

        public TransfersApiTests()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<ITransfersApi>() as TransfersApi;
        }

        [Fact]
        public void UserServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<TransfersApi>>();
            var authServiceMock = new Mock<IAuthApi>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new TransfersApi(config, _loggerMock.Object, authServiceMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }

        [Fact]
        public void Test_Transfers_Create()
        {
            UserNaturalDTO john = TestHelper.GetJohn();
            var wallet = TestHelper.GetNewJohnsWalletWithMoney(10000);

            TransferDTO transfer = TestHelper.GetNewTransfer(wallet);
            WalletDTO creditedWallet = TestHelper.WalletsApi.Get(transfer.CreditedWalletId).Result;

            Assert.True(transfer.Id.Length > 0);
            Assert.Equal(transfer.AuthorId, john.Id);
            Assert.Equal(transfer.CreditedUserId, john.Id);
            Assert.Equal(100, creditedWallet.Balance.Amount);
        }

        [Fact]
        public void Test_Transfers_Get()
        {
            UserNaturalDTO john = TestHelper.GetJohn();
            var wallet = TestHelper.GetNewJohnsWalletWithMoney(10000);
            TransferDTO transfer = TestHelper.GetNewTransfer(wallet);

            TransferDTO getTransfer = _objectToTest.Get(transfer.Id).Result;

            Assert.Equal(transfer.Id, getTransfer.Id);
            Assert.Equal(getTransfer.AuthorId, john.Id);
            Assert.Equal(getTransfer.CreditedUserId, john.Id);
            TestHelper.AssertEqualInputProps(transfer, getTransfer);
        }

        [Fact]
        public void Test_Transfers_CreateRefund()
        {
            WalletDTO wallet = TestHelper.GetNewJohnsWalletWithMoney(10000);
            TransferDTO transfer = TestHelper.GetNewTransfer(wallet);
            WalletDTO walletBefore = TestHelper.WalletsApi.Get(wallet.Id).Result;

            RefundDTO refund = TestHelper.GetNewRefundForTransfer(transfer);
            WalletDTO walletAfter = TestHelper.WalletsApi.Get(wallet.Id).Result;

            Assert.True(refund.Id.Length > 0);
            Assert.True(refund.DebitedFunds.Amount == transfer.DebitedFunds.Amount);
            Assert.True(walletBefore.Balance.Amount == (walletAfter.Balance.Amount - transfer.DebitedFunds.Amount));
            Assert.Equal(TransactionType.TRANSFER, refund.Type);
            Assert.Equal(TransactionNature.REFUND, refund.Nature);
        }
    }
}