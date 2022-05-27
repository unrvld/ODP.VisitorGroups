using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class OrderLikelihoodCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(OrderLikelihoodSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(OrderLikelihoodSelectionFactory))]
#endif
        [Required]
        public string OrderLikelihood { get; set; }
    }
}