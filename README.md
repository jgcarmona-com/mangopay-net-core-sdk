
MANGOPAY .NET CORE SDK 
=================================================
MangopayNetCoreSDK is a Microsoft .NET Core client library to work with
[Mangopay REST API](http://docs.mangopay.com/api-references/).

![Build](https://github.com/JuanGarciaCarmona/mangopay-net-core-sdk/workflows/Build/badge.svg)

License
-------------------------------------------------
The code is distributed as is and with no warranty. Use it at your own risk and feel free to improve the library acording to MIT license. See [LICENSE file](https://github.com/JuanGarciaCarmona/mangopay-net-core-sdk/blob/master/LICENSE).

Installation and package dependencies
-------------------------------------------------
It is an extremely easy process, only two steps:

1.- Add Mangopay configuration settings in appsettings.json file, it should look similar to:

      "MangoPayApiConfiguration": {
        "ClientId": "sdk-unit-tests",
        "ClientPassword": "cqFfFrWfCcb7UadHNxx2C9Lo6Djw8ZduLi7J9USTmu8bhxxpju",
        "BaseUrl": "https://api.sandbox.mangopay.com",
        "Timeout": 0,
        "ApiVersion": "v2.01"
      }

2.- In Startup class, resolve IConfiguration and in ConfigureServices method, call services.AddMangoPayServices(_configuration) extension method.


Unit Tests
-------------------------------------------------
Tests are placed in MangoPay.SDK.Tests project in solution.


Account creation
-------------------------------------------------
You can get yourself a [free sandbox account](https://www.mangopay.com/signup/create-sandbox/) or sign up for a [production account](https://www.mangopay.com/signup/production-account/) (note that validation of your production account can take a few days, so think about doing it in advance of when you actually want to go live).


Configuration
-------------------------------------------------
TBD


Sample usage (get, update and save an entity)
-------------------------------------------------
TBD
   
