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
        private bool disposedValue;

        public GraphQLClient(OdpVisitorGroupOptions options)
        {
            _apiKey = options.PrivateApiKey;
            _graphQlClient = new GraphQLHttpClient(options.EndPoint, new NewtonsoftJsonSerializer());
        }

        public async Task<T> Query<T>(string query)where T : class
        {
            var request = new AuthencatedGraphQLHttpRequest(this._apiKey)
            {
                Query = query
            };

#if NET461_OR_GREATER
            /* This is needed as the alternate code fails to complete, without an error.*/
            var response461 = _graphQlClient.SendQueryAsync<T>(request);
            Task.WaitAll(response461);

            return response461.Result.Data;
#elif NET5_0
            var response = await _graphQlClient.SendQueryAsync<T>(request);
            return response.Data;
#endif
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
