using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class OrderLikelihoodCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(OrderLikelihoodSelectionFactory)
        )]

        [Required]
        public string OrderLikelihood { get; set; }
    }
}