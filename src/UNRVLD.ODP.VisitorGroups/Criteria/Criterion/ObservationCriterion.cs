using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Http;


using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Configuration;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "Query specific observations about the current user (calculated every 24 hours)",
        DisplayName = "Observation"
    )]
    public class ObservationCriterion : OdpCriterionBase<ObservationCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;


        public ObservationCriterion(OdpVisitorGroupOptions optionValues,
                            ICustomerDataRetriever customerDataRetriever,
                            IODPUserProfile odpUserProfile)
            : base(odpUserProfile)
        {
            _optionValues = optionValues;
            _customerDataRetriever = customerDataRetriever;
        }

        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var vuidValue = OdpUserProfile.GetDeviceId(httpContext);
            return !string.IsNullOrEmpty(vuidValue) && IsMatchInner(principal, vuidValue);
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
                    var customer = _customerDataRetriever.GetCustomerInfo(vuidValue, Model.InstanceName);
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