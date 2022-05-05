
#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

using System;
using System.Globalization;
using EPiServer.Personalization.VisitorGroups;

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
        private readonly IODPUserProfile _odpUserProfile;

#if NET5_0_OR_GREATER
        public CustomerPropertyTextCriterion(OdpVisitorGroupOptions optionValues, 
            ICustomerDataRetriever customerDataRetriever,
            IODPUserProfile odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
            _odpUserProfile = odpUserProfile;
        }

        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            return this.IsMatchInner(principal, httpContext);
        }
#elif NET461_OR_GREATER
        public CustomerPropertyTextCriterion()
        {
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
            _customerDataRetriever = ServiceLocator.Current.GetInstance<ICustomerDataRetriever>();
            _odpUserProfile = ServiceLocator.Current.GetInstance<IODPUserProfile>();
        }

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            return this.IsMatchInner(principal, httpContext.ApplicationInstance.Context);
        }
#endif

        private bool IsMatchInner(IPrincipal principal, HttpContext httpContext)
        {
            try
            {
                if (_optionValues.IsConfigured == false)
                {
                    return false;
                }

                var vuidValue = _odpUserProfile.DeviceId;

                if (!string.IsNullOrEmpty(vuidValue))
                {
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
                                isMatch = propertyValue.Equals(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
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
                                isMatch = propertyValue.ToLower().Contains(Model.PropertyValue.ToLower());
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
