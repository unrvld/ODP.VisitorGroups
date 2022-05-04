using System;
using System.Linq;
using EPiServer.Framework.Cache;
using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
#if NET5_0_OR_GREATER
using UNRVLD.ODP.VisitorGroups.REST;
#endif

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class CustomerDataRetriever : ICustomerDataRetriever
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache;
#if NET5_0_OR_GREATER
        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;
#endif
        public CustomerDataRetriever(IGraphQLClient graphQlClient, OdpVisitorGroupOptions optionValues, ISynchronizedObjectInstanceCache cache
#if NET5_0_OR_GREATER
            , ICustomerPropertyListRetriever customerPropertyListRetriever
#endif
            )
        {
            _graphQlClient = graphQlClient;
            _optionValues = optionValues;
            _cache = cache;
#if NET5_0_OR_GREATER
            _customerPropertyListRetriever = customerPropertyListRetriever;
#endif
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

        public dynamic GetCustomerInfoDynamic(string vuidValue)
        {
            try
            {
                var cacheKey = $"odp_rts_customer_dynamic_{vuidValue}";
                var cachedResult = _cache.Get(cacheKey);
                if (cachedResult != null)
                {
                    return (Customer)cachedResult;
                }

#if NET5_0_OR_GREATER
                var allFields = _customerPropertyListRetriever.GetCustomerProperties().ToList();
                var allFieldsString = String.Join(System.Environment.NewLine, allFields.Select(x => x.name));
#else
                var allFieldsString = "";
#endif

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

                var result = _graphQlClient.Query<DynamicCustomerResponse>(query).Result;

                if (result.Customer != null)
                {
                    // Use a micro cache approach to improve performance if the same VG is used multiple times on a page
                    _cache.Insert(
                        cacheKey,
                        result,
                        new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _optionValues.CacheTimeoutSeconds),
                            CacheTimeoutType.Absolute));
                }

                return result.Customer;
            }
            catch
            {
                return null;
            }
        }

    }

    public class DynamicCustomerResponse
    {
        [JsonProperty("customer")]
        public dynamic Customer { get; set; }
    }

}