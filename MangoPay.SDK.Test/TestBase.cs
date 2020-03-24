using MangoPay.SDK.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MangoPay.SDK.Test
{
    public abstract class TestBase
    {       
        protected MangoPayApiConfiguration GetConfiguration()
        {
            return new MangoPayApiConfiguration
            {
                ClientId = "sdk-unit-tests",
                ClientPassword = "cqFfFrWfCcb7UadHNxx2C9Lo6Djw8ZduLi7J9USTmu8bhxxpju",
                BaseUrl = "https://api.sandbox.mangopay.com",
                ApiVersion = "v2.01",
            };
        }

        protected MangoPayApiConfiguration GetApplicationConfiguration()
        {
            return TestHelper.GetApplicationConfiguration();
        }
    }
}
