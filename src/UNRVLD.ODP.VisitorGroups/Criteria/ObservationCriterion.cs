using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query specific observations about the current user (calculated every 24 hours)",
        DisplayName = "Observation"
    )]
    public class ObservationCriterion : CriterionBase<ObservationCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

#if NET5_0_OR_GREATER
        public ObservationCriterion(OdpVisitorGroupOptions optionValues, ICustomerDataRetriever customerDataRetriever) 
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

                    var isMatch = false;
                    switch (Model.Observation)
                    {
                        case "TotalRevenue":
                            isMatch = CompareMe(customer.Observations?.TotalRevenue, Model.Comparison);
                            break;
                        case "OrderCount":
                            isMatch = CompareMe(customer.Observations?.OrderCount, Model.Comparison);
                            break;
                        case "AverageOrderRevenue":
                            isMatch = CompareMe(customer.Observations?.AverageOrderRevenue, Model.Comparison);
                            break;
                    }

                    return isMatch;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

#elif NET461_OR_GREATER
        public ObservationCriterion()
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

                    var isMatch = false;
                    switch (Model.Observation)
                    {
                        case "TotalRevenue":
                            isMatch = CompareMe(customer.Observations?.TotalRevenue, Model.Comparison);
                            break;
                        case "OrderCount":
                            isMatch = CompareMe(customer.Observations?.OrderCount, Model.Comparison);
                            break;
                        case "AverageOrderRevenue":
                            isMatch = CompareMe(customer.Observations?.AverageOrderRevenue, Model.Comparison);
                            break;
                    }

                    return isMatch;

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
                    return value < Model.ObservationValue;
                case "EqualTo":
                    return value == Model.ObservationValue;
                case "GreaterThan":
                    return value > Model.ObservationValue;
                default:
                    return false;
            }
        }
    }
}