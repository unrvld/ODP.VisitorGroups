using System;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.GraphQL;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

using System.Security.Principal;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using System.Linq;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
      Category = "Data platform",
      Description = "Matches the user to a specific segment",
      DisplayName = "Is In Segment"
        )]
    public class AudienceCriterion : CriterionBase<AudienceCriterionModel>
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ISynchronizedObjectInstanceCache _cache;

#if NET5_0_OR_GREATER
        public AudienceCriterion(IGraphQLClient graphQlClient, OdpVisitorGroupOptions optionValues, ISynchronizedObjectInstanceCache cache)
        {
            _graphQlClient = graphQlClient;
            _optionValues = optionValues;
            _cache = cache;
        }

        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            try
            {
                if (_optionValues.IsConfigured == false)
                {
                    return false;
                }

                if (httpContext.Request.Cookies.ContainsKey(_optionValues.OdpCookieName))
                {
                    var vuidValue = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                    if (!string.IsNullOrWhiteSpace(vuidValue))
                    {
                        vuidValue = vuidValue.Substring(0, 36).Replace("-", "");
                    }

                    return IsInAudience(vuidValue, Model.Audience);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

#elif NET461_OR_GREATER
        public AudienceCriterion()
        {
            _graphQlClient = ServiceLocator.Current.GetInstance<IGraphQLClient>();
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
            _cache = ServiceLocator.Current.GetInstance<ISynchronizedObjectInstanceCache>();
        }

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            try
            {
                if (_optionValues.IsConfigured == false)
                {
                    return false;
                }

                var cookie = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    var vuidValue = cookie.Value.Substring(0, 36).Replace("-", "");
                    return IsInAudience(vuidValue, Model.Audience);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

#endif

        private bool IsInAudience(string vuidValue, string audience)
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

                var result = _graphQlClient.Query<CustomerResponse>(query).Result;
                var isInAudience = result.Customer.Response.EdgeItems?.Any() ?? false;

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
