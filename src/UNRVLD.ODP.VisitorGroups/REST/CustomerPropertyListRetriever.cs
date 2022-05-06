using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.REST.Models;

using RestSharp;
using System.Linq;
using System.Threading;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public class CustomerPropertyListRetriever : ICustomerPropertyListRetriever, IDisposable
    {
        private readonly RestClient _restClient;
        private readonly OdpVisitorGroupOptions _options;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private bool disposedValue;

        public CustomerPropertyListRetriever(//RestClient restClient, 
            OdpVisitorGroupOptions options, 
            ISynchronizedObjectInstanceCache cache)
        {
            _restClient = new RestClient(options.BaseEndPoint);
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

            var apiResult = GetCustomerPropertiesRequest();

            if (apiResult == null || !apiResult.Any())
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

        private IEnumerable<Field> GetCustomerPropertiesRequest()
        {
            var request = new RestRequest("/v3/schema/objects/customers");
            request.AddHeader("x-api-key", _options.PrivateApiKey);

            var response =  _restClient.GetAsync<CustomerFieldsResponse>(request).Result;
                
            return response?.fields ?? Enumerable.Empty<Field>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    _restClient.Dispose();
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