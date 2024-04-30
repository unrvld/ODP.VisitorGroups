using System;
using System.Threading.Tasks;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    /// <summary>
    /// Represents a GraphQL client that can be used to send queries and receive responses.
    /// </summary>
    public interface IGraphQLClient : IDisposable
    {
        /// <summary>
        /// Sends a GraphQL query and returns the response of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="query">The GraphQL query string.</param>
        /// <returns>The response object of type T.</returns>
        Task<T?> Query<T>(string query) where T : class;
    }
}