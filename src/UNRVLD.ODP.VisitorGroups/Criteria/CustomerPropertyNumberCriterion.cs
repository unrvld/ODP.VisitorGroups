using EPiServer.Personalization.VisitorGroups;

using System.Security.Principal;
using Newtonsoft.Json.Linq;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;


using Microsoft.AspNetCore.Http;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query customer number fields",
        DisplayName = "Customer property (number)"
    )]
    public class CustomerPropertyNumberCriterion : OdpCriterionBase<CustomerPropertyNumberCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public CustomerPropertyNumberCriterion(OdpVisitorGroupOptions optionValues, 
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
                    var customer = _customerDataRetriever.GetCustomerInfo(vuidValue);
                    if (customer == null)
                    {
                        return false;
                    }
                    
                    if (!customer.AdditionalFields.TryGetValue(Model.PropertyName, out var propertyToken))
                    {
                        return false;
                    }

                    return decimal.TryParse(propertyToken?.Value<string>(), out var propertyValue) &&
                           CompareMe(propertyValue, Model.Comparison);
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
