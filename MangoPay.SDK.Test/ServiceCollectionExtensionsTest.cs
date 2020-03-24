using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.Clients;
using MangoPay.SDK.APIs.Users;
using MangoPay.SDK.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class ServiceCollectionExtensionsTest
    {
        private ServiceCollection _services; 
        private IConfigurationRoot _configuration;
        private ServiceProvider _provider ;

        public ServiceCollectionExtensionsTest()
        {
            // Arrange: 
            _services = new ServiceCollection();
            _configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            _services.AddMangoPayServices(_configuration);
            _provider = _services.BuildServiceProvider();
        }

        [Fact]
        public void WhenAddingMangiPayServices_ItShouldRegisterAuthApi()
        {           
            var authApi = _provider.GetRequiredService<IAuthApi>();
            Assert.NotNull(authApi);
        }

        [Fact]
        public void WhenAddingMangiPayServices_ItShouldRegisterUsersApi()
        {
            var usersApi = _provider.GetRequiredService<IUsersApi>();
            Assert.NotNull(usersApi);
        }

        [Fact]
        public void WhenAddingMangiPayServices_ItShouldRegisterClientsApi()
        {
            var clientsApi = _provider.GetRequiredService<IClientsApi>();
            Assert.NotNull(clientsApi);
        }
    }
}
