using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class EngagementRankCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }


        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(NumberComparisonSelectionFactory)
        )]

        [Required]
        public string Comparison { get; set; }


        [CriterionPropertyEditor(Order = 20)]

        [Required]
        public int EngagementRank { get; set; }
    }
}