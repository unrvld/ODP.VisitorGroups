using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using System.Net.Http;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    internal class AuthencatedGraphQLHttpRequest : GraphQLHttpRequest
    {
        private readonly string apiKey;
        public AuthencatedGraphQLHttpRequest(string apiKey)
        {
            this.apiKey=apiKey;
        }

        public override HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
        {
            var request = base.ToHttpRequestMessage(options, serializer);

            request.Headers.Add("x-api-key", apiKey);

            return request;
        }
    }


}
