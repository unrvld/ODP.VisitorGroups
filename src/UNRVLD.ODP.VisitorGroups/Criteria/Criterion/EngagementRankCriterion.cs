using EPiServer.Personalization.VisitorGroups;

using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Customer engagement rank from 0 to 100 (calculated every 24 hours)",
        DisplayName = "Engagement rank"
    )]
    public class EngagementRankCriterion : OdpCriterionBase<EngagementRankCriterionModel>
    {
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public EngagementRankCriterion(OdpVisitorGroupOptions optionValues,
                ICustomerDataRetriever customerDataRetriever,
                IODPUserProfile odpUserProfile)
            : base(optionValues,odpUserProfile)
        {
            _customerDataRetriever = customerDataRetriever;
        }

        protected override bool IsMatchInner(IPrincipal principal, string vuidValue)
        {
            try
            {
                var customer = _customerDataRetriever.GetCustomerInfo(vuidValue, Model.InstanceName);

                return CompareMe(customer?.Insights?.EngagementRank, Model.Comparison);
            }
            catch
            {
                return false;
            }
        }

        private bool CompareMe(decimal? value, string comparison)
        {
            if (value == null)
            {
                return false;
            }

            switch (comparison)
            {
                case "LessThan":
                    return value < Model.EngagementRank;
                case "EqualTo":
                    return value == Model.EngagementRank;
                case "NotEqualTo":
                    return value == Model.EngagementRank;
                case "GreaterThan":
                    return value > Model.EngagementRank;
                default:
                    return false;
            }
        }
    }
}