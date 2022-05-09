using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

using System.Security.Principal;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "How likely is the user going to order (calculated every 24 hours)",
        DisplayName = "Order Likelihood"
    )]
    public class OrderLikelihoodCriterion : CriterionBase<OrderLikelihoodCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;
        private readonly IODPUserProfile _odpUserProfile;

#if NET5_0_OR_GREATER
        public OrderLikelihoodCriterion(OdpVisitorGroupOptions optionValues, 
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
        public OrderLikelihoodCriterion()
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

                    return customer.Insights?.OrderLikelihood == Model.OrderLikelihood;
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