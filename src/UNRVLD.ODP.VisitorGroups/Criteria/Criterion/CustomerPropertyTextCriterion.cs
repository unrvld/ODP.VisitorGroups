using System;
using EPiServer.Personalization.VisitorGroups;
using System.Security.Principal;
using Newtonsoft.Json.Linq;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
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
        private readonly IPrefixer _prefixer;

        public CustomerPropertyTextCriterion(OdpVisitorGroupOptions optionValues,
            ICustomerDataRetriever customerDataRetriever,
            IODPUserProfile odpUserProfile,
            IPrefixer prefixer)
            : base(odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
            _prefixer = prefixer;
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
                    var splitPrefix = _prefixer.SplitPrefix(Model.PropertyName);

                    var customer = _customerDataRetriever.GetCustomerInfo(vuidValue, splitPrefix.prefix);
                    if (customer == null)
                    {
                        return false;
                    }

                    if (!customer.AdditionalFields.TryGetValue(Model.PropertyName, out var propertyToken))
                    {
                        return false;
                    }

                    var propertyValue = propertyToken?.Value<string>();

                    switch (Model.Comparison)
                    {
                        case "HasNoValue":
                            return string.IsNullOrEmpty(propertyValue);
                        case "HasValue":
                            return !string.IsNullOrEmpty(propertyValue);
                        case "Is":
                            if (propertyValue != null)
                            {
                                return propertyValue.Equals(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
                            }

                            break;
                        case "StartsWith":
                            if (propertyValue != null)
                            {
                                return propertyValue.StartsWith(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
                            }

                            break;
                        case "Contains":
                            if (propertyValue != null)
                            {
#if NET5_0_OR_GREATER
                                return propertyValue.Contains(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
#else
                                return propertyValue.IndexOf(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase) >= 0;
#endif
                            }

                            break;
                        case "EndsWith":
                            if (propertyValue != null)
                            {
                                return propertyValue.EndsWith(Model.PropertyValue, StringComparison.CurrentCultureIgnoreCase);
                            }

                            break;
                    }
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