using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

using System.Security.Principal;

namespace UNRVLD.ODP.VisitorGroups.Criteria
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
        

#if NET5_0_OR_GREATER
        public WinbackZoneCriterion(OdpVisitorGroupOptions optionValues, 
                            ICustomerDataRetriever customerDataRetriever,
                            IODPUserProfile odpUserProfile)
            : base(odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
        }
#elif NET461_OR_GREATER
        public WinbackZoneCriterion()
        {
            _customerDataRetriever = ServiceLocator.Current.GetInstance<ICustomerDataRetriever>();
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
            OdpUserProfile = ServiceLocator.Current.GetInstance<IODPUserProfile>();
        }
#endif

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
                    var customer = _customerDataRetriever.GetCustomerInfo(vuidValue);
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