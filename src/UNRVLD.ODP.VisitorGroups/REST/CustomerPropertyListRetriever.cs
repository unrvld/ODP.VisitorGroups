#if NET5_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.REST.Models;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public class CustomerPropertyListRetriever : ICustomerPropertyListRetriever
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OdpVisitorGroupOptions _options;
        private readonly ISynchronizedObjectInstanceCache _cache;

        public CustomerPropertyListRetriever(IHttpClientFactory httpClientFactory, OdpVisitorGroupOptions options, ISynchronizedObjectInstanceCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _cache = cache;
        }

        //public CustomerPropertyListRetriever()
        //{
        //    _httpClientFactory = ServiceLocator.Current.GetInstance<IHttpClientFactory>();
        //    _options = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
        //}

        public IEnumerable<Field> GetCustomerProperties()
        {
            var cacheKey = $"odp_rts_customer_schema";
            var cachedResult = _cache.Get(cacheKey);
            if (cachedResult != null)
            {
                return (IEnumerable<Field>)cachedResult;
            }

            var apiResult = GetCustomerPropertiesAsync().Result;

            if (apiResult == null)
            {
                return null;
            }

            _cache.Insert(
                cacheKey,
                apiResult,
                new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _options.SchemaCacheTimeoutSeconds),
                    CacheTimeoutType.Absolute));

            return apiResult;
        }

        private async Task<IEnumerable<Field>> GetCustomerPropertiesAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                _options.BaseEndPoint + "/v3/schema/objects/customers")
            {
                Headers =
                {
                    { "x-api-key", _options.PrivateApiKey }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var customerFields = await JsonSerializer.DeserializeAsync
                    <CustomerFieldsResponse>(contentStream);

                if (customerFields != null)
                {
                    return customerFields.fields;
                }
            }

            return null;
        }

    }
}
#endif