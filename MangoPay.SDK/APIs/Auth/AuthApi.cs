using MangoPay.SDK.APIs.Auth.DTOs;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Auth
{
    /// <summary>
    /// Auth Service that implements oAuth 2.0 flow
    /// It stores the Auth Token so it MUST be a singleton instance
    /// </summary>
    /// <seealso cref="IAuthApi" />
    public class AuthApi : IAuthApi
    {
        private readonly ILogger<AuthApi> _logger;
        private readonly MangoPayApiConfiguration _config;
        private OAuthTokenDTO _token;

        private HttpClient _client { get; }

        public AuthApi(
            MangoPayApiConfiguration config,
            ILogger<AuthApi> logger)
        {
            _config = config;
            _logger = logger;
            _client = new HttpClient()
            {
                BaseAddress = new Uri(_config.BaseUrl)
            };
        }

        public async Task<OAuthTokenDTO> GetAuthToken()
        {
            if (_token == null || _token.IsExpired())
            {
                return await Authenticate();
            }
            else
            {
                return _token;
            }
        }

        private async Task<OAuthTokenDTO> Authenticate()
        {
            _logger.LogInformation("Authenticating");
            HttpRequestMessage requestToken = PrepareAuthenticationRequest();

            var response = await _client.SendAsync(requestToken);
            if (response.IsSuccessStatusCode)
            {
                return await StoreAndReturnToken(response);
            }
            else
            {
                await HandleError(response);
            }
            return null;
        }

        private HttpRequestMessage PrepareAuthenticationRequest()
        {
            var url = $"{_config.BaseUrl}/{_config.ApiVersion}/oauth/token/";
            var secret = $"{_config.ClientId}:{_config.ClientPassword}";
            var encodedSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes(secret));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedSecret);

            var authenticationRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent("grant_type=client_credentials")
            };

            authenticationRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
            authenticationRequest.Headers.TryAddWithoutValidation("Authorization", $"Basic {encodedSecret}");
            return authenticationRequest;
        }

        private async Task<OAuthTokenDTO> StoreAndReturnToken(HttpResponseMessage response)
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            _token = JsonSerializer.DeserializeAsync<OAuthTokenDTO>(responseStream).Result;

            _logger.LogInformation("Authenticating");
            return _token;
        }

        private async Task HandleError(HttpResponseMessage response)
        {
            var responseCode = (int)response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();
            if (responseCode == 401)
                throw new UnauthorizedAccessException(content);
            else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                throw new TimeoutException(content);
            else
                throw new ResponseException(content, responseCode);
        }
    }
}
