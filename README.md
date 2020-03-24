

MANGOPAY .NET CORE SDK 
=================================================
MangopayNetCoreSDK is a Microsoft .NET Core client library to work with
[Mangopay REST API](http://docs.mangopay.com/api-references/).

![Build](https://github.com/JuanGarciaCarmona/mangopay-net-core-sdk/workflows/Build/badge.svg)

License
-------------------------------------------------
The code is distributed as is and with no warranty. Use it at your own risk and feel free to improve the library acording to MIT license. See [LICENSE file](https://github.com/JuanGarciaCarmona/mangopay-net-core-sdk/blob/master/LICENSE).

Motivation
-------------------------------------------------
I had to implement this library in order to implement its integration within a personal project (my own StartUp, indeed). I want you to be aware on that despite the integration efforts Mangopay thought that my business was too risky to support me so I had to go on another direction and integrate a different payments provider with marketplace capabilities. What a shame!

Design
-------------------------------------------------
You can use a set of clients to call MangoPay API, every client implements it's dedicated Interface and inherits from BaseApi, that is, indeed, the most important class within this library.
BaseApi Requires an instance of IAuthApi, the client that implements Authentication and gets authentication tokes to be injected in headers according to Mangopay documentation.
The rest of the clients are mostly exact as the original "old" .Net Library.

Three steps integration
-------------------------------------------------
It is a three steps process:

1.- Install [Mangopay.NetCore.SDK](https://www.nuget.org/packages/Mangopay.NetCore.SDK) 

2.- Add Mangopay configuration settings in appsettings.json file, it should look similar to:

      "MangoPayApiConfiguration": {
        "ClientId": "sdk-unit-tests",
        "ClientPassword": "cqFfFrWfCcb7UadHNxx2C9Lo6Djw8ZduLi7J9USTmu8bhxxpju",
        "BaseUrl": "https://api.sandbox.mangopay.com",
        "Timeout": 0,
        "ApiVersion": "v2.01"
      }

3.- In Startup class, resolve an IConfiguration object and use it in ConfigureServices method by call services.AddMangoPayServices(_configuration) extension method.

    // Add MangoPay Client
    services.AddMangoPayServices(_configuration);
 
 **Notice:** Take a look on how this extension method looks like because the library is not using all the original clients, I had no need to implement all the logic so those clients are empty/not implemented. Also, You might want to implement your own extension method.
 
    public static IServiceCollection AddMangoPayServices(
	    this IServiceCollection services, 
	    IConfiguration configuration)
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
 
Unit Tests
-------------------------------------------------
Tests are placed in MangoPay.SDK.Tests project in solution.

Account creation
-------------------------------------------------
You can get yourself a [free sandbox account](https://www.mangopay.com/signup/create-sandbox/) or sign up for a [production account](https://www.mangopay.com/signup/production-account/) (note that validation of your production account can take a few days, so think about doing it in advance of when you actually want to go live).

Sample usage (get, update and save an entity)
-------------------------------------------------
Best way to understand how to use this library is taking a look to its IntegrationTest project, in example, UserApiTest can be a great starting point.

    public UsersApiTest()
    {
        // Arrange (simulate Startup configuration): 
        var services = new ServiceCollection();
        var configuration = TestHelper.GetIConfigurationRoot();           
        services.AddMangoPayServices(configuration);
        var provider = services.BuildServiceProvider();

        // Resolve an instante of IUserApi (it could be used in a controller or a BL Service)
        _objectToTest = provider.GetRequiredService<IUsersApi>() as UserApi;
    }
 How to create a Natural User:

    [Fact]
    public void WhenCreatingValidNaturalUser_ItShouldReturnNaturalUserDto()
    {
        // Arrange:
        var user = new UserNaturalPostDTO(
            "john.doe@sample2.org", // Etc...
		    // MANY OTHER PROPERTIES
            
            // Act:
            var result = _objectToTest.Create(user).Result;

            // Assert:
            Assert.NotNull(result);
            Assert.IsType<UserNaturalDTO>(result);
            Assert.True(!string.IsNullOrEmpty(result.Id));
        }
Create a Legal User would be pretty similar:

     [Fact]
     public void WhenCreatingValidLegalUser_ItShouldReturnLegalUserDto()
     {
          // Arrange:
          UserLegalPostDTO user = new UserLegalPostDTO(
           "john.doe@sample.org",// Etc...
		    // MANY OTHER PROPERTIES          
    
          // Act:
          var result = _objectToTest.Create(user).Result;
    
          // Assert:
          Assert.NotNull(result);
          Assert.IsType<UserLegalDTO>(result);
          Assert.True(!string.IsNullOrEmpty(result.Id));
     }

Get a Paginated List of Users:

    [Fact]
    public void WhenGettingAllUsers_ItShouldReturnAPaginatedListOfUSers()
    {        
        // Act:
        var result = _objectToTest.GetAll().Result;

        // Assert:
        Assert.NotNull(result);
        Assert.IsType<ListPaginated<UserDTO>>(result);
    }
Save a Legal User:

    [Fact]
    public void Test_Users_Save_Legal()
    {
        try
        {
            UserLegalDTO matrix = TestHelper.GetMatrix();

            UserLegalPutDTO matrixPut = new UserLegalPutDTO
            {               
                LegalRepresentativeLastName = matrix.LegalRepresentativeLastName + " - CHANGED",
                // Etc...
			    // MANY OTHER PROPERTIES
            };

            UserLegalDTO userSaved = _objectToTest.UpdateLegal(matrixPut, matrix.Id).Result;
            UserLegalDTO userFetched = _objectToTest.GetLegal(userSaved.Id).Result;

            Assert.Equal(matrixPut.LegalRepresentativeLastName, userFetched.LegalRepresentativeLastName);
            TestHelper.AssertEqualInputProps(userSaved, userFetched);
        }
        catch (Exception ex)
        {
            Assert.True(false, ex.Message);
        }
    }
Update a Bank Account:

     [Fact]
            public void Test_Users_UpdateBankAccount()
            {
                try
                {
                    UserNaturalDTO john = TestHelper.GetJohn();
                    BankAccountIbanDTO account = TestHelper.GetJohnsAccount();
    
                    Assert.True(account.Id.Length > 0);
                    Assert.True(account.UserId == (john.Id));
                    Assert.True(account.Active);
    
                    // disactivate bank account
                    DisactivateBankAccountPutDTO disactivateBankAccount = new DisactivateBankAccountPutDTO();
                    disactivateBankAccount.Active = false;
    
                    BankAccountDTO result = _objectToTest.UpdateBankAccount(john.Id, disactivateBankAccount, account.Id).Result;
    
                    Assert.NotNull(result);
                    Assert.True(account.Id == result.Id);
                    Assert.False(result.Active);
                }
                catch (Exception ex)
                {
                    Assert.True(false, ex.Message);
                }
            }
I really hope this library to save you time with Mangopay integration.
