using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.PayOuts;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class PayOutsApiTests : TestBase
    {
        private PayOutsApi _objectToTest;
        private Mock<ILogger<PayOutsApi>> _loggerMock;

        public PayOutsApiTests()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IPayOutsApi>() as PayOutsApi;
        }

        [Fact]
        public void UserServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<PayOutsApi>>();
            var authServiceMock = new Mock<IAuthApi>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new PayOutsApi(config, _loggerMock.Object, authServiceMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }
        [Fact]
        public void Test_PayOuts_Create_BankWire()
        {
            try
            {
                PayInDTO payIn = TestHelper.GetJohnsPayInCardWeb();
                PayOutDTO payOut = TestHelper.GetJohnsPayOutBankWire();

                Assert.True(payOut.Id.Length > 0);
                Assert.Equal(PayOutPaymentType.BANK_WIRE, payOut.PaymentType);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}