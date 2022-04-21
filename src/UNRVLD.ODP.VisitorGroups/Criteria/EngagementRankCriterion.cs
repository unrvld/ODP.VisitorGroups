using EPiServer.Personalization.VisitorGroups;

#if NET5_0
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
        Description = "Customer engagement rank from 0 to 100 (calculated every 24 hours)",
        DisplayName = "Engagement rank"
    )]
    public class EngagementRankCriterion : CriterionBase<EngagementRankCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

#if NET5_0
        public EngagementRankCriterion(OdpVisitorGroupOptions optionValues, ICustomerDataRetriever customerDataRetriever) 
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

                    return CompareMe(customer.Insights?.EngagementRank, Model.Comparison);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

#elif NET461_OR_GREATER
        public EngagementRankCriterion()
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

                    return CompareMe(customer.Insights?.EngagementRank, Model.Comparison);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

#endif

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