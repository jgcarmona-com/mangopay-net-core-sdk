using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.BankAliases;
using MangoPay.SDK.APIs.CardPreAuthorizations;
using MangoPay.SDK.APIs.CardRegistrations;
using MangoPay.SDK.APIs.Cards;
using MangoPay.SDK.APIs.Clients;
using MangoPay.SDK.APIs.Events;
using MangoPay.SDK.APIs.Hooks;
using MangoPay.SDK.APIs.Idempotency;
using MangoPay.SDK.APIs.Kyc;
using MangoPay.SDK.APIs.Mandates;
using MangoPay.SDK.APIs.PayIns;
using MangoPay.SDK.APIs.PayOuts;
using MangoPay.SDK.APIs.PermissionGroups;
using MangoPay.SDK.APIs.Refunds;
using MangoPay.SDK.APIs.Reports;
using MangoPay.SDK.APIs.SingleSignOns;
using MangoPay.SDK.APIs.Transfers;
using MangoPay.SDK.APIs.UboDeclarations;
using MangoPay.SDK.APIs.Users;
using MangoPay.SDK.APIs.Wallets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MangoPay.SDK.Core
{
    public static class MangoPayServiceCollectionExtensions
    {
        public static IServiceCollection AddMangoPayServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            services.AddSingleton<IAuthApi, AuthApi>();
            services.AddTransient<IBankingAliasApi, BankingAliasApi>();
            services.AddTransient<ICardPreAuthorizationsApi, CardPreAuthorizationsApi>();
            services.AddTransient<ICardRegistrationsApi, CardRegistrationsApi>();
            services.AddTransient<ICardsApi, CardsApi>();
            services.AddTransient<IClientsApi, ClientsApi>();
            services.AddTransient<IEventsApi, EventsApi>();
            services.AddTransient<IHooksApi, HooksApi>();
            //services.AddTransient<IIdempotencyApi, IdempotencyApi>();
            services.AddTransient<IKycApi, KycApi>();
            services.AddTransient<IMandatesApi, MandatesApi>();
            services.AddTransient<IPayInsApi, PayInsApi>();
            services.AddTransient<IPayOutsApi, PayOutsApi>();
            //services.AddTransient<IPermissionGroupsApi, PermissionsGroupsApi>();
            services.AddTransient<IRefundsApi, RefundsApi>();
            services.AddTransient<IReportsApi, ReportsApi>();
            //services.AddTransient<ISingleSignOnsApi, SingleSignOnsApi>();
            services.AddTransient<ITransfersApi, TransfersApi>();
            //services.AddTransient<IUboDeclarationsApi, null>();
            services.AddTransient<IUsersApi, UserApi>();
            services.AddTransient<IWalletsApi, WalletsApi>();
            var config = new MangoPayApiConfiguration();
            configuration.Bind("MangoPayApiConfiguration", config);
            services.AddSingleton(config);
            return services;
        }
    }
}
