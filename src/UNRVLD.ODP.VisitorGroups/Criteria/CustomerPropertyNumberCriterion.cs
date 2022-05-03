#if NET5_0
using EPiServer.Personalization.VisitorGroups;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query customer number fields",
        DisplayName = "Customer property (number)"
    )]
    public class CustomerPropertyNumberCriterion : CriterionBase<CustomerPropertyNumberCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public CustomerPropertyNumberCriterion(OdpVisitorGroupOptions optionValues, ICustomerDataRetriever customerDataRetriever)
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

                    var customer = _customerDataRetriever.GetCustomerInfoDynamic(vuidValue);
                    if (customer == null)
                    {
                        return false;
                    }

                    var isMatch = false;

                    var rawValue = (customer[Model.PropertyName] as Newtonsoft.Json.Linq.JValue)?.Value;
                    if (rawValue != null)
                    {
                        decimal propertyValue;
                        if (decimal.TryParse(rawValue.ToString(), out propertyValue))
                        {
                            isMatch = CompareMe(propertyValue, Model.Comparison);
                        }
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

        private bool CompareMe(decimal value, string comparison)
        {
            switch (comparison)
            {
                case "LessThan":
                    return value < Model.PropertyValue;
                case "EqualTo":
                    return value == Model.PropertyValue;
                case "GreaterThan":
                    return value > Model.PropertyValue;
                default:
                    return false;
            }
        }
    }
}
#endif
