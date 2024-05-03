using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public class GraphQLClient : IGraphQLClient, IDisposable
    {
        private readonly string _apiKey;
        private readonly GraphQLHttpClient _graphQlClient;
        private readonly bool _isConfigured;
        private readonly ILogger<GraphQLClient> _logger;
        private bool disposedValue;

        public GraphQLClient(OdpEndpoint endPoint, ILogger<GraphQLClient> logger)
        {
            _apiKey = endPoint.PrivateApiKey.Trim();
            _isConfigured = endPoint.IsConfigured;
            _graphQlClient = new GraphQLHttpClient(endPoint.BaseEndPoint.Trim() + "/v3/graphql", new NewtonsoftJsonSerializer());
            _logger = logger;
        }

        public async Task<T?> Query<T>(string query) where T : class
        {
            if (!_isConfigured)
            {
                return default;
            }

            try {
                var request = new AuthencatedGraphQLHttpRequest(this._apiKey)
                {
                    Query = query
                };

                var response = await _graphQlClient.SendQueryAsync<T>(request);
                return response.Data;
            } catch (Exception ex) {
                _logger.LogError(ex, "Error querying GraphQL");
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _graphQlClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
