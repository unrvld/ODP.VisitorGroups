﻿using System;
using System.Linq;
using EPiServer.Framework.Cache;
using Microsoft.Extensions.Logging;
using UNRVLD.ODP.VisitorGroups.Configuration;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using UNRVLD.ODP.VisitorGroups.REST;


namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class CustomerDataRetriever : ICustomerDataRetriever
    {
        private readonly IGraphQLClientFactory _graphQLClientFactory;
        private readonly ILogger<CustomerDataRetriever> _logger;
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache;

        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;
        private readonly IPrefixer _prefixer;

        public CustomerDataRetriever(IGraphQLClientFactory graphQLClientFactory,
            ILogger<CustomerDataRetriever> logger,
            OdpVisitorGroupOptions optionValues,
            ISynchronizedObjectInstanceCache cache,
            ICustomerPropertyListRetriever customerPropertyListRetriever,
            IPrefixer prefixer)
        {
            _graphQLClientFactory = graphQLClientFactory;
            _logger = logger;
            _optionValues = optionValues;
            _cache = cache;
            _customerPropertyListRetriever = customerPropertyListRetriever;
            _prefixer = prefixer;
        }

        public Customer? GetCustomerInfo(string vuidValue, string? endpointKey = null)
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

                var allFields = _customerPropertyListRetriever.GetCustomerProperties(endpointKey);

                var allFieldsString = string.Join(Environment.NewLine, allFields.Select(x =>  _prefixer.SplitPrefix(x.Name).value));

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

                Customer? customer =  null;
                var odpEndPoint = _optionValues.GetEndpoint(endpointKey);

                if (odpEndPoint != null)
                {
                    var graphQlClient = _graphQLClientFactory.Get(odpEndPoint);
                    var result = graphQlClient.Query<CustomerResponse>(query).Result;

                    customer = result?.Customer;
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer info");
                return null;
            }
        }

        public bool IsInAudience(string vuidValue, string audience, string? endpointKey = null)
        {
            try
            {
                var cacheKey = $"odp_rts_{vuidValue}_{audience}";
                var cachedResult = _cache.Get(cacheKey);
                if (cachedResult != null)
                {
                    return (bool)cachedResult;
                }

                var query = $@"query MyQuery {{
                                      customer(vuid: ""{vuidValue}"") {{
                                        audiences (subset: [""{audience}""]) {{
                                          edges {{
                                            node {{
                                              name
                                            }}
                                          }}
                                        }}
                                      }}
                                    }}";

                var isInAudience = false;

                var odpEndPoint = _optionValues.GetEndpoint(endpointKey);

                if (odpEndPoint is not null)
                {
                    var graphQlClient = _graphQLClientFactory.Get(odpEndPoint);
                    var result = graphQlClient.Query<CustomerResponse>(query).Result;

                    isInAudience = result?.Customer?.Response?.EdgeItems.Count == 0;
                }

                // Use a micro cache approach to improve performance if the same VG is used multiple times on a page
                _cache.Insert(
                    cacheKey,
                    isInAudience,
                    new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _optionValues.CacheTimeoutSeconds), CacheTimeoutType.Absolute));

                return isInAudience;
            }
            catch
            {
                return false;
            }
        }
    }
}