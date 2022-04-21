using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class EngagementRankCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(ComparisonSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(ComparisonSelectionFactory))]
#endif
        [Required]
        public string Comparison { get; set; }

#if NET5_0
        [CriterionPropertyEditor(Order = 20)]
#endif
        [Required]
        public int EngagementRank { get; set; }
    }
}