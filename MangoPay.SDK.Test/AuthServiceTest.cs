using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class AuthServiceTest : TestBase
    {
        private AuthApi _objectToTest;
        private Mock<ILogger<AuthApi>> _loggerMock;

        public AuthServiceTest()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IAuthApi>() as AuthApi;
        }

        [Fact]
        public void AuthServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<AuthApi>>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new AuthApi(config, _loggerMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }


        [Fact]
        public void WhenGettingAuthToken_ItShould_TO_BE_DEFINED()
        {
            // Arrange:

            // Act:
           var token = _objectToTest.GetAuthToken().Result ;
            // Assert:
            Assert.NotNull(token);
        }
    }
}
