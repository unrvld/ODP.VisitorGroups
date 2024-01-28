using EPiServer.Personalization.VisitorGroups;


using Microsoft.AspNetCore.Http;


using System.Security.Principal;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "How likely is the user going to order (calculated every 24 hours)",
        DisplayName = "Order Likelihood"
    )]
    public class OrderLikelihoodCriterion : OdpCriterionBase<OrderLikelihoodCriterionModel>
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        private readonly ICustomerDataRetriever _customerDataRetriever;
        
        public OrderLikelihoodCriterion(OdpVisitorGroupOptions optionValues, 
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