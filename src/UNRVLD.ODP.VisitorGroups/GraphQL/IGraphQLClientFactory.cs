using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    /// <summary>
    /// Represents a factory for creating instances of <see cref="IGraphQLClient"/>.
    /// </summary>
    public interface IGraphQLClientFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IGraphQLClient"/> for the specified OdpEndpoint.
        /// </summary>
        /// <param name="endPoint">The OdpEndpoint to create the client for.</param>
        /// <returns>An instance of <see cref="IGraphQLClient"/>.</returns>
        IGraphQLClient Get(OdpEndpoint endPoint);
    }
}