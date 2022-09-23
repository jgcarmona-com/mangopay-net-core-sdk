using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.Wallets;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class WalletsApiTests : TestBase
    {
        private WalletsApi _objectToTest;
        private Mock<ILogger<WalletsApi>> _loggerMock;
        public WalletsApiTests()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IWalletsApi>() as WalletsApi;
        }

        [Fact]
        public void UserServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<WalletsApi>>();
            var authServiceMock = new Mock<IAuthApi>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new WalletsApi(config, _loggerMock.Object, authServiceMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }
        [Fact(Skip = "To be reviewed")]
        public void Test_Wallets_Create()
        {
            UserNaturalDTO john = TestHelper.GetJohn();
            WalletDTO wallet = TestHelper.GetJohnsWallet();

            Assert.True(wallet.Id.Length > 0);
            Assert.Contains(john.Id, wallet.Owners);
        }

        [Fact]
        public void Test_Wallets_Get()
        {
            UserNaturalDTO john = TestHelper.GetJohn();
            WalletDTO wallet = TestHelper.GetJohnsWallet();

            WalletDTO getWallet = _objectToTest.Get(wallet.Id).Result;

            Assert.Equal(wallet.Id, getWallet.Id);
            Assert.Contains(john.Id, wallet.Owners);
        }

        [Fact]
        public void Test_Wallets_Save()
        {
            WalletDTO wallet = TestHelper.GetJohnsWallet();
            WalletPutDTO walletPut = new WalletPutDTO();
            walletPut.Description = wallet.Description + " - changed";
            walletPut.Owners = wallet.Owners;
            walletPut.Tag = wallet.Tag;

            WalletDTO saveWallet = _objectToTest.Update(walletPut, wallet.Id).Result;

            Assert.Equal(wallet.Id, saveWallet.Id);
            Assert.Equal(wallet.Description + " - changed", saveWallet.Description);
        }

        [Fact]
        public void Test_Wallets_Transactions()
        {
            UserNaturalDTO john = TestHelper.GetJohn();

            WalletDTO wallet = TestHelper.CreateJohnsWallet();
            PayInDTO payIn = TestHelper.CreateJohnsPayInCardWeb(wallet.Id);

            Pagination pagination = new Pagination(1, 1);
            FilterTransactions filter = new FilterTransactions();
            filter.Type = TransactionType.PAYIN;
            ListPaginated<TransactionDTO> transactions = _objectToTest.GetTransactions(wallet.Id, pagination, filter, null).Result;

            Assert.True(transactions.Count == 1);
            Assert.True(transactions[0] is TransactionDTO);
            Assert.Equal(transactions[0].AuthorId, john.Id);
            TestHelper.AssertEqualInputProps(transactions[0], payIn);
        }

        [Fact(Skip = "To be reviewed")]
        public async Task Test_Wallets_Transactions_With_Sorting()
        {
            WalletDTO wallet = TestHelper.GetJohnsWallet();

            // create 2 payins
            TestHelper.GetJohnsPayInCardWeb();
            System.Threading.Thread.Sleep(1000);
            TestHelper.GetNewPayInCardWeb();
            Sort sort = new Sort();
            sort.AddField("CreationDate", SortDirection.desc);
            Pagination pagination = new Pagination(1, 20);
            FilterTransactions filter = new FilterTransactions();
            filter.Type = TransactionType.PAYIN;

            ListPaginated<TransactionDTO> transactions = await _objectToTest.GetTransactions(wallet.Id, pagination, filter, sort);

            Assert.True(transactions[0].CreationDate > transactions[1].CreationDate);
        }
    }
}

