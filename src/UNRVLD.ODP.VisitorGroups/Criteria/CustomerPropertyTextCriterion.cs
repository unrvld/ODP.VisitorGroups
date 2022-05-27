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

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query customer text fields",
        DisplayName = "Customer property (text)"
    )]
    public class CustomerPropertyTextCriterion : OdpCriterionBase<CustomerPropertyTextCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

#if NET5_0_OR_GREATER
        public CustomerPropertyTextCriterion(OdpVisitorGroupOptions optionValues,
            ICustomerDataRetriever customerDataRetriever,
            IODPUserProfile odpUserProfile)
            : base(odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
        }
#elif NET461_OR_GREATER
        public CustomerPropertyTextCriterion()
        {
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
            _customerDataRetriever = ServiceLocator.Current.GetInstance<ICustomerDataRetriever>();
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
                                isMatch = propertyValue.Equals(Model.PropertyValue,
                                    StringComparison.CurrentCultureIgnoreCase);
                            }

                            break;
                        case "StartsWith":
                            if (propertyValue != null)
                            {
                                isMatch = propertyValue.StartsWith(Model.PropertyValue, true,
                                    CultureInfo.CurrentCulture);
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
