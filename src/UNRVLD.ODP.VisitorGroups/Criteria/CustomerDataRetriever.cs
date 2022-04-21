using System;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class CustomerDataRetriever : ICustomerDataRetriever
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache;

        public CustomerDataRetriever(IGraphQLClient graphQlClient, OdpVisitorGroupOptions optionValues, ISynchronizedObjectInstanceCache cache)
        {
            _graphQlClient = graphQlClient;
            _optionValues = optionValues;
            _cache = cache;
        }

        public Customer GetCustomerInfo(string vuidValue)
        {
            try
            {
                var cacheKey = $"odp_rts_customer_{vuidValue}";
                var cachedResult = _cache.Get(cacheKey);
                if (cachedResult != null)
                {
                    return (Customer)cachedResult;
                }

                var query = $@"query MyQuery {{
                                  customer(vuid: ""{vuidValue}"") {{
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
