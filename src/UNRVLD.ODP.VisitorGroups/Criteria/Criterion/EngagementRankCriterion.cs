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
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public EngagementRankCriterion(OdpVisitorGroupOptions optionValues,
                ICustomerDataRetriever customerDataRetriever,
                IODPUserProfile odpUserProfile)
            : base(odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
        }

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
                    var customer = _customerDataRetriever.GetCustomerInfo(vuidValue, Model.InstanceName);
                    if (customer == null)
                    {
                        return false;
                    }

                    return CompareMe(customer.Insights?.EngagementRank, Model.Comparison);
                }
            }
            catch
            {
                return false;
            }
            return false;
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
                case "GreaterThan":
                    return value > Model.EngagementRank;
                default:
                    return false;
            }
        }
    }
}