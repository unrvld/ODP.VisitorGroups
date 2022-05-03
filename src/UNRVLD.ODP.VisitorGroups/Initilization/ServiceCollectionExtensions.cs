#if NET5_0
using Microsoft.Extensions.DependencyInjection;
using UNRVLD.ODP.VisitorGroups.Criteria;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.REST;

namespace UNRVLD.ODP.VisitorGroups.Initilization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddODPVisitorGroups(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IGraphQLClient, GraphQLClient>();
            services.AddScoped<ICustomerDataRetriever, CustomerDataRetriever>();
            services.AddScoped<ICustomerPropertyListRetriever, CustomerPropertyListRetriever>();
        }
    }
}
#endif
