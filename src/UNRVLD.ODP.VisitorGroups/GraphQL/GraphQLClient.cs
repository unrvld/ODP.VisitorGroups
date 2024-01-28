using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Threading.Tasks;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public class GraphQLClient : IGraphQLClient, IDisposable
    {
        private readonly string _apiKey;
        private readonly GraphQLHttpClient _graphQlClient;
        private readonly OdpVisitorGroupOptions _options;
        private bool disposedValue;

        public GraphQLClient(OdpVisitorGroupOptions options)
        {
            _apiKey = options.PrivateApiKey;
            _options = options;
            _graphQlClient = new GraphQLHttpClient(options.BaseEndPoint + "/v3/graphql", new NewtonsoftJsonSerializer());
        }

        public async Task<T> Query<T>(string query) where T : class
        {
            if (!_options.IsConfigured)
            {
                return default;
            }

            var request = new AuthencatedGraphQLHttpRequest(this._apiKey)
            {
                Query = query
            };

            var response = await _graphQlClient.SendQueryAsync<T>(request);
            return response.Data;
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
