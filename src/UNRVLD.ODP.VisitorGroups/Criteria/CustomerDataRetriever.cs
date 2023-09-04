using System;
using System.Linq;
using EPiServer.Framework.Cache;
using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using UNRVLD.ODP.VisitorGroups.REST;


namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class CustomerDataRetriever : ICustomerDataRetriever
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache;

        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;

        public CustomerDataRetriever(IGraphQLClient graphQlClient,
            OdpVisitorGroupOptions optionValues,
            ISynchronizedObjectInstanceCache cache,
            ICustomerPropertyListRetriever customerPropertyListRetriever)
        {
            _graphQlClient = graphQlClient;
            _optionValues = optionValues;
            _cache = cache;
            _customerPropertyListRetriever = customerPropertyListRetriever;
        }

        public Customer GetCustomerInfo(string vuidValue)
        {
            try
            {
                if (!_optionValues.IsConfigured)
                {
                    return null;
                }


                var cacheKey = $"odp_rts_customer_{vuidValue}";
                var cachedResult = _cache.Get(cacheKey);
                if (cachedResult != null)
                {
                    return (Customer)cachedResult;
                }

                var allFields = _customerPropertyListRetriever.GetCustomerProperties();
                var allFieldsString = String.Join(Environment.NewLine, allFields.Select(x => x.name));

                var query = $@"query MyQuery {{
                                  customer(vuid: ""{vuidValue}"") {{
                                    {allFieldsString}
                                    observations {{
                                      total_revenue
                                      order_count
                                      average_order_revenue
                                    }}
                                    insights {{
                                      engagement_rank
                                      winback_zone
                                      order_likelihood
                                    }}
                                  }}
                                }}";

                var result = _graphQlClient.Query<CustomerResponse>(query).Result;
                var customer = result?.Customer;

                if (customer != null)
                {
                    // Use a micro cache approach to improve performance if the same VG is used multiple times on a page
                    _cache.Insert(
                        cacheKey,
                        customer,
                        new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _optionValues.CacheTimeoutSeconds),
                            CacheTimeoutType.Absolute));
                }

                return customer;
            }
            catch
            {
                return null;
            }
        }
    }
}