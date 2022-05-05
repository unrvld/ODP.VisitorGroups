
using EPiServer.Personalization.VisitorGroups;

using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

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
        private readonly IODPUserProfile _odpUserProfile;


#if NET5_0_OR_GREATER
        public CustomerPropertyNumberCriterion(OdpVisitorGroupOptions optionValues, 
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
        public CustomerPropertyNumberCriterion()
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


        public bool IsMatchInner(IPrincipal principal, HttpContext httpContext)
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
