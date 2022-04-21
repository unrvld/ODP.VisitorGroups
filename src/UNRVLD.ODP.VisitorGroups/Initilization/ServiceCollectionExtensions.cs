#if NET5_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;
using UNRVLD.ODP.VisitorGroups.Criteria;
using UNRVLD.ODP.VisitorGroups.GraphQL;

namespace UNRVLD.ODP.VisitorGroups.Initilization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddODPVisitorGroups(this IServiceCollection services)
        {
            services.AddScoped<IGraphQLClient, GraphQLClient>();
            services.AddScoped<ICustomerDataRetriever, CustomerDataRetriever>();
        }
    }
}
#endif
