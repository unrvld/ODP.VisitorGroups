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
    public class WinbackZoneCriterion : CriterionBase<WinbackZoneCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

#if NET5_0_OR_GREATER
        public WinbackZoneCriterion(OdpVisitorGroupOptions optionValues, ICustomerDataRetriever customerDataRetriever)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
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

#elif NET461_OR_GREATER
        public WinbackZoneCriterion()
        {
            _customerDataRetriever = ServiceLocator.Current.GetInstance<ICustomerDataRetriever>();
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
        }

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            try
            {
                var cookie = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                if (cookie != null)
                {
                    var vuidValue = cookie.Value.Substring(0, 36).Replace("-", "");

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
#endif
    }
}