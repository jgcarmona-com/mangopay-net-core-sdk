using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Exceptions;
using MangoPay.SDK.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs
{
    /// <summary> 
    /// Base class for all Api classes.
    /// </summary>
    public abstract class BaseApi
    {
        protected string _baseUrl;
        protected readonly MangoPayApiConfiguration _config;
        protected readonly ILogger<BaseApi> _logger;
        private readonly IAuthApi _auth;

        /// <summary>
        /// Creates new API instance.
        /// </summary>
        /// <param name="root">Root/parent instance that holds the OAuthToken and Configuration instance.</param>
        public BaseApi(MangoPayApiConfiguration config,
            ILogger<BaseApi> logger,
            IAuthApi auth)
        {
            _config = config;
            _logger = logger;
            _auth = auth;
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <typeparam name="U">Return type.</typeparam>
        /// <typeparam name="T">Type on behalf of which the request is being called.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected async Task<U> CreateEntity<U, T>(string path, T entity, string idempotencyKey = null)
            where U : EntityBase, new()
            where T : EntityPostBase
        {
            var token = await _auth.GetAuthToken();
            if (token != null)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                using var httpContent = CreateHttpContent(entity);
                using var request = new HttpRequestMessage(HttpMethod.Post, path)
                {
                    Content = httpContent
                };

                if (!string.IsNullOrWhiteSpace(idempotencyKey))
                    request.Headers.Add(Constants.IDEMPOTENCY_KEY, idempotencyKey);

                using var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    return DeserializeJsonFromStream<U>(stream);
                }
                else
                {
                    await HandleApiError(response, stream);
                }
            }
            return null;
        }


        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">Type on behalf of which the request is being called.</typeparam>
        /// <param name="targetUrl">The target URL.</param>
        /// <returns>
        /// The DTO instance returned from API.
        /// </returns>
        protected async Task<T> GetEntity<T>(string targetUrl) where T : class, new()
        {
            var token = await _auth.GetAuthToken();
            if (token != null)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                using var request = new HttpRequestMessage(HttpMethod.Get, targetUrl);

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    return DeserializeJsonFromStream<T>(stream);
                }
                else
                {
                    await HandleApiError(response, stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected async Task<U> UpdateEntity<U, T>(string targetUrl, T entity)
            where U : EntityBase, new()
            where T : EntityPutBase
        {
            var token = await _auth.GetAuthToken();
            if (token != null)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                using var request = new HttpRequestMessage(HttpMethod.Put, targetUrl);

                using var httpContent = CreateHttpContent(entity);
                request.Content = httpContent;

                using var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    return DeserializeJsonFromStream<U>(stream);
                }
                else
                {
                    await HandleApiError(response, stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a Paginated List of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        protected Task<ListPaginated<T>> GetList<T>(string targetUrl, Pagination pagination, Sort sort = null)
            where T : class, new()
        {
            return GetList<T>(targetUrl, pagination, sort, additionalUrlParams: null);
        }

        /// <summary>
        /// Gets a Paginated List of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="additionalUrlParams">The additional URL parameters.</param>
        /// <param name="entitiesId">The entities identifier.</param>
        /// <returns></returns>
        protected async Task<ListPaginated<T>> GetList<T>(string targetUrl, Pagination pagination, Sort sort, Dictionary<string, string> additionalUrlParams, string idempotencyKey = null)
            where T : class, new()
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }
            else
            {
                targetUrl += string.Format("{0}page={1}&per_page={2}", Constants.URI_QUERY_SEPARATOR, pagination.Page, pagination.ItemsPerPage);
            }

            if (sort != null && sort.IsSet)
            {
                if (additionalUrlParams == null)
                    additionalUrlParams = new Dictionary<string, string>();

                additionalUrlParams.Add(Constants.SORT_URL_PARAMETER_NAME, sort.GetFields());
            }

            if (additionalUrlParams != null)
            {
                string parameters = "";
                foreach (KeyValuePair<string, string> entry in additionalUrlParams)
                {
                    parameters += string.Format("&{0}={1}", Uri.EscapeDataString(entry.Key), Uri.EscapeDataString(entry.Value));
                }
                if (pagination == null)
                    parameters = parameters.Remove(0, 1).Insert(0, Constants.URI_QUERY_SEPARATOR);

                targetUrl += parameters;
            }

            var token = await _auth.GetAuthToken();
            if (token != null)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");
                using var request = new HttpRequestMessage(HttpMethod.Get, targetUrl);
                if (!string.IsNullOrWhiteSpace(idempotencyKey))
                    request.Headers.Add(Constants.IDEMPOTENCY_KEY, idempotencyKey);
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    return DeserializeJsonFromStream<ListPaginated<T>>(stream);
                }
                else
                {
                    await HandleApiError(response, stream);
                }
            }
            return null;
        }

        #region Private Helper Methods

        /// <summary>
        /// Handles an API error.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="TimeoutException"></exception>
        /// <exception cref="MangoPay.SDK.Core.Exceptions.ResponseException"></exception>
        private async Task HandleApiError(HttpResponseMessage response, Stream stream)
        {
            var responseCode = (int)response.StatusCode;
            var content = await StreamToStringAsync(stream);

            if (responseCode == 401)
                throw new UnauthorizedAccessException(content);
            else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                throw new TimeoutException(content);
            else
                throw new ResponseException(content, responseCode);
        }     

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true);
            using var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None };
            var js = new JsonSerializer();
            js.Serialize(jtw, value);
            jtw.Flush();
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);
            var js = new JsonSerializer();
            var searchResult = js.Deserialize<T>(jtr);
            return searchResult;
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        #endregion
    }
}
