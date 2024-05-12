using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;
using UNRVLD.ODP.VisitorGroups.Criteria;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.REST;

namespace UNRVLD.ODP.VisitorGroups.Initilization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddODPVisitorGroups(this IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddScoped<IPrefixer, StandardPrefixer>();
            services.AddScoped<IGraphQLClientFactory, GraphQLClientFactory>();
            services.AddScoped<ICustomerDataRetriever, CustomerDataRetriever>();
            services.AddScoped<ICustomerPropertyListRetriever, CustomerPropertyListRetriever>();
            services.AddHttpContextOrThreadScoped<IODPUserProfile, ODPUserProfile>();
            services.AddTransient<IAudienceSizeCachePopulator, AudienceSizeCachePopulator>();
        }
    }
}

