using EPiServer.Personalization.VisitorGroups;


using System.Security.Principal;
using UNRVLD.ODP.VisitorGroups.Configuration;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Criterion
{
    [VisitorGroupCriterion(
        Category = "Data platform",
        Description = "How likely is the user going to order (calculated every 24 hours)",
        DisplayName = "Order Likelihood"
    )]
    public class OrderLikelihoodCriterion : OdpCriterionBase<OrderLikelihoodCriterionModel>
    {
        private readonly ICustomerDataRetriever _customerDataRetriever;

        public OrderLikelihoodCriterion(OdpVisitorGroupOptions optionValues,
                                        ICustomerDataRetriever customerDataRetriever,
                                        IODPUserProfile odpUserProfile)
            : base(optionValues,odpUserProfile)
        {
            _customerDataRetriever = customerDataRetriever;
        }

        protected override bool IsMatchInner(IPrincipal principal, string vuidValue)
        {
            try
            {
                var customer = _customerDataRetriever.GetCustomerInfo(vuidValue, Model.InstanceName);

                return customer?.Insights?.OrderLikelihood == Model.OrderLikelihood;
            
            }
            catch
            {
                return false;
            }
        }
    }
}