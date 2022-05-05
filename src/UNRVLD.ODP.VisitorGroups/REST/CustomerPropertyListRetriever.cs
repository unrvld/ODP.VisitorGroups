using System;
using System.Collections.Generic;

using System.Net.Http;

#if NET5_0_OR_GREATER
using System.Text.Json;

#elif NET461_OR_GREATER
using Newtonsoft.Json;
#endif

using System.Threading.Tasks;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.REST.Models;
using System.IO;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public class CustomerPropertyListRetriever : ICustomerPropertyListRetriever
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OdpVisitorGroupOptions _options;
        private readonly ISynchronizedObjectInstanceCache _cache;

        public CustomerPropertyListRetriever(IHttpClientFactory httpClientFactory, 
            OdpVisitorGroupOptions options, 
            ISynchronizedObjectInstanceCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _cache = cache;
        }

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
            CustomerFieldsResponse customerFields;

            using (var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                _options.BaseEndPoint + "/v3/schema/objects/customers")
            {
                Headers =
                {
                    { "x-api-key", _options.PrivateApiKey }
                }
            })    
                using(var httpClient = _httpClientFactory.CreateClient())
                using(var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
                { 
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using (var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync())
                    
    #if NET5_0_OR_GREATER
                    customerFields = await JsonSerializer.DeserializeAsync
                        <CustomerFieldsResponse>(contentStream);
    #elif NET461_OR_GREATER
                    using (var sr = new StreamReader(contentStream))
                    using (var reader = new JsonTextReader(sr))
                    {
                        var serializer = new JsonSerializer();
                        customerFields = serializer.Deserialize<CustomerFieldsResponse>(reader);
                    }
    #endif
                    if (customerFields != null)
                    {
                        return customerFields.fields;
                    }
                }

                return null;
            }
        }
    }
}