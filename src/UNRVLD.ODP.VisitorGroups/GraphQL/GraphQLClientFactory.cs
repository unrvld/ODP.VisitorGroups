using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public class GraphQLClientFactory : IGraphQLClientFactory, IDisposable
    {
        protected ConcurrentDictionary<string, IGraphQLClient> Clients = new();
        private bool disposedValue;
        private readonly ILoggerFactory _loggerFactory;

        public GraphQLClientFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public IGraphQLClient Get(OdpEndpoint endPoint) => Clients.GetOrAdd(endPoint.Name, _ => new GraphQLClient(endPoint, _loggerFactory.CreateLogger<GraphQLClient>()));

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