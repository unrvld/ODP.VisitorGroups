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
        private readonly IODPUserProfile _odpUserProfile;

#if NET5_0_OR_GREATER
        public ObservationCriterion(OdpVisitorGroupOptions optionValues, 
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
        public ObservationCriterion()
        {
            _customerDataRetriever = ServiceLocator.Current.GetInstance<ICustomerDataRetriever>();
            _optionValues = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
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