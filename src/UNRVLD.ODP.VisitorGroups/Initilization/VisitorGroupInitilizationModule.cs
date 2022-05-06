#if NET461_OR_GREATER

using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.Criteria;
using UNRVLD.ODP.VisitorGroups.REST;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace UNRVLD.ODP.VisitorGroups.Initilization
{
    [InitializableModule]
    [ModuleDependency(typeof(FrameworkInitialization))]
    public class VisitorGroupInitilizationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;

            services.AddScoped<ICustomerDataRetriever, CustomerDataRetriever>();

            // No need to configure OdpVisitorGroupOptions as any classes attributed with [Options] are taken care of by the CMS
            // init on CMS v11.17 and above. Values for the those settings can in appSettings as follows:
            // <add key="episerver:setoption:UNRVLD.ODP.OdpVisitorGroupOptions.OdpCookieName,UNRVLD.ODP.VisitorGroups" value="vuid" />
            // Full information here: https://world.optimizely.com/documentation/Release-Notes/ReleaseNote/?releaseNoteId=CMS-15875

            services.AddScoped<IGraphQLClient, GraphQLClient>();
            services.AddScoped<ICustomerDataRetriever, CustomerDataRetriever>();
            services.AddScoped<ICustomerPropertyListRetriever, CustomerPropertyListRetriever>();
            services.AddHttpContextOrThreadScoped<IODPUserProfile, ODPUserProfile>();
        }

        public void Initialize(InitializationEngine context)
        {

        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
#endif
