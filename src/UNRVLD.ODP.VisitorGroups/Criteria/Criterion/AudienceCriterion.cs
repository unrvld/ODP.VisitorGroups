using System;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.GraphQL;

using System.Security.Principal;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
{
    [VisitorGroupCriterion(
      Category = "Data platform",
      Description = "Matches the user to a specific segment",
      DisplayName = "Is In Segment"
        )]
    public class AudienceCriterion(IGraphQLClientFactory graphQLClientFactory,
                            OdpVisitorGroupOptions optionValues,
                            ISynchronizedObjectInstanceCache cache,
                            IODPUserProfile odpUserProfile,
                            IPrefixer prefixer) : OdpCriterionBase<AudienceCriterionModel>(odpUserProfile)
    {
        private readonly IGraphQLClientFactory _graphQLClientFactory = graphQLClientFactory;
        private readonly OdpVisitorGroupOptions _optionValues = optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache = cache;
        private readonly IPrefixer prefixer = prefixer;

        protected override bool IsMatchInner(IPrincipal principal, string vuidValue)
        {
            try
            {
                if (_optionValues.IsConfigured == false)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(vuidValue))
                {
                    if (_optionValues.HasMultipleEndpoints)
                    {
                        var splitPrefix = prefixer.SplitPrefix(Model.Audience);
                    
                        return IsInAudience(vuidValue,  splitPrefix.value, splitPrefix.prefix);
                    } 
                    else
                        return IsInAudience(vuidValue, Model.Audience);

                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        private bool IsInAudience(string vuidValue, string audience, string? endpointKey = null)
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
