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
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;
        public WinbackZoneCriterion(OdpVisitorGroupOptions optionValues,
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

                    return customer.Insights?.WinbackZone == Model.WinbackZone;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}