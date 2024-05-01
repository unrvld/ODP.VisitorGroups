using EPiServer.Personalization.VisitorGroups;


using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Configuration;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "What winback state is the user in (calculated every 24 hours)",
        DisplayName = "Winback zone"
    )]
    public class WinbackZoneCriterion : OdpCriterionBase<WinbackZoneCriterionModel>
    {
        private readonly ICustomerDataRetriever _customerDataRetriever;
        public WinbackZoneCriterion(OdpVisitorGroupOptions optionValues,
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

                return customer?.Insights?.WinbackZone == Model.WinbackZone;
            }
            catch
            {
                return false;
            }
        }
    }
}