#if NET5_0_OR_GREATER
using System;
using System.Globalization;
using EPiServer.Personalization.VisitorGroups;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query customer text fields",
        DisplayName = "Customer property (text)"
    )]
    public class CustomerPropertyTextCriterion : CriterionBase<CustomerPropertyTextCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public CustomerPropertyTextCriterion(OdpVisitorGroupOptions optionValues, ICustomerDataRetriever customerDataRetriever)
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
                    var propertyValue = (customer[Model.PropertyName] as Newtonsoft.Json.Linq.JValue)?.Value as string;

                    switch (Model.Comparison)
                    {
                        case "HasNoValue":
                            isMatch = string.IsNullOrEmpty(propertyValue);
                            break;
                        case "HasValue":
                            isMatch = !string.IsNullOrEmpty(propertyValue);
                            break;
                        case "Is":
                            if (propertyValue != null)
                            {
                                isMatch = propertyValue.Equals(Model.PropertyValue);
                            }
                            break;
                        case "StartsWith":
                            if (propertyValue != null)
                            {
                                isMatch = propertyValue.StartsWith(Model.PropertyValue, true, CultureInfo.CurrentCulture);
                            }
                            break;
                        case "Contains":
                            if (propertyValue != null)
                            {
                                isMatch = propertyValue.Contains(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
                            }
                            break;
                        case "EndsWith":
                            if (propertyValue != null)
                            {
                                isMatch = propertyValue.EndsWith(Model.PropertyValue, true, CultureInfo.CurrentCulture);
                            }
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
    }
}
#endif
