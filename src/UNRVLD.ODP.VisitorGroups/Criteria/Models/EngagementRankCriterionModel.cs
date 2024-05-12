using System.ComponentModel.DataAnnotations;
using EPiServer.Data.Dynamic;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class EngagementRankCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }


        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(OdpInstanceSelectionFactory)
        )]

        [Required]
        public string InstanceName { get; set; } = string.Empty;

        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(NumberComparisonSelectionFactory)
        )]

        [Required]
        public string Comparison { get; set; } = string.Empty;


        [CriterionPropertyEditor(Order = 30)]

        [Required]
        public int EngagementRank { get; set; }
    }
}