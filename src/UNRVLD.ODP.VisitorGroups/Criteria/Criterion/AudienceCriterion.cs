using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.GraphQL;

using System.Security.Principal;
using EPiServer.Framework.Cache;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
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
                            ICustomerDataRetriever customerDataRetriever,
                            IPrefixer prefixer) : OdpCriterionBase<AudienceCriterionModel>(optionValues, odpUserProfile)
    {
        private readonly IGraphQLClientFactory _graphQLClientFactory = graphQLClientFactory;
        private readonly ISynchronizedObjectInstanceCache _cache = cache;
        private readonly ICustomerDataRetriever _customerDataRetriever = customerDataRetriever;
        private readonly IPrefixer _prefixer = prefixer;

        protected override bool IsMatchInner(IPrincipal principal, string vuidValue)
        {
            try
            {
                if (OptionValues.HasMultipleEndpoints)
                {
                    var splitPrefix = _prefixer.SplitPrefix(Model.Audience);
                
                    return _customerDataRetriever.IsInAudience(vuidValue,  splitPrefix.value, splitPrefix.prefix);
                } 
                else
                {
                    return _customerDataRetriever.IsInAudience(vuidValue, Model.Audience);
                }
            }
            catch
            {
                return false;
            }
        }
 
    }
}
