using System;
using System.Collections.Concurrent;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public class GraphQLClientFactory : IGraphQLClientFactory, IDisposable
    {
        protected ConcurrentDictionary<string, IGraphQLClient> Clients = new();
        private bool disposedValue;

        public IGraphQLClient Get(OdpEndpoint endPoint)
        {
            if (Clients.TryGetValue(endPoint.Name, out var client))
            {
                return client;
            }

            var newClient = new GraphQLClient(endPoint);
            Clients.TryAdd(endPoint.Name, newClient);

            return newClient;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var client in Clients)
                    {
                        client.Value.Dispose();
                    }
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