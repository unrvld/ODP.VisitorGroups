using System;
using System.Collections.Generic;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.REST.Models;

using RestSharp;
using System.Linq;
using UNRVLD.ODP.VisitorGroups.Configuration;
using UNRVLD.ODP.VisitorGroups.Criteria;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public class CustomerPropertyListRetriever : ICustomerPropertyListRetriever, IDisposable
    {
        private readonly OdpVisitorGroupOptions _options;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly IPrefixer _prefixer;
        private bool disposedValue;

        public CustomerPropertyListRetriever(
            OdpVisitorGroupOptions options, 
            ISynchronizedObjectInstanceCache cache,
            IPrefixer prefixer)
        {
            _options = options;
            _cache = cache;
            _prefixer = prefixer;
        }

        public IEnumerable<Field> GetCustomerProperties(string? endpointName)
        {
            var cacheKey = $"odp_rts_customer_schema_{endpointName}";
            var cachedResult = _cache.Get(cacheKey);
            if (cachedResult != null)
            {
                return (IEnumerable<Field>)cachedResult;
            }

            List<Field> apiResult = [];

            var endpoint = _options.GetEndpoint(endpointName);

            if (endpoint != null)
            {
                var results = GetCustomerPropertiesRequest(_options.HasMultipleEndpoints, endpoint);

                if (results != null && results.Count != 0)
                {
                    apiResult.AddRange(results);
                }
            }

            if (apiResult == null || !apiResult.Any())
            {
                return [];
            }

            _cache.Insert(
                cacheKey,
                apiResult,
                new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _options.SchemaCacheTimeoutSeconds),
                    CacheTimeoutType.Absolute));

            return apiResult;
        }

        private ICollection<Field> GetCustomerPropertiesRequest(bool hasMultipleEndpoints, OdpEndpoint odpEndpoint)
        {
            try
            {
                using var _restClient = new RestClient(new Uri(odpEndpoint.BaseEndPoint), useClientFactory: true);

                var request = new RestRequest("/v3/schema/objects/customers");
                request.AddHeader("x-api-key", odpEndpoint.PrivateApiKey);

                var response = _restClient.GetAsync<CustomerFieldsResponse>(request).Result;

                response?.Fields.ToList().ForEach(x => x.DisplayName = hasMultipleEndpoints ? _prefixer.Prefix(x.DisplayName,odpEndpoint.Name ) : x.DisplayName);
                response?.Fields.ToList().ForEach(x => x.Name = hasMultipleEndpoints ? _prefixer.Prefix(x.Name,odpEndpoint.Name ) : x.Name);
        
                return response?.Fields ?? Array.Empty<Field>();
            }
            catch
            {
                return Array.Empty<Field>();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CustomerPropertyListRetriever()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}