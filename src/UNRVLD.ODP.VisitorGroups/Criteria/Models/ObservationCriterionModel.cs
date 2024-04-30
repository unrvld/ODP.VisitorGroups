using System.ComponentModel.DataAnnotations;
using EPiServer.Data.Dynamic;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class ObservationCriterionModel : CriterionModelBase
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
            SelectionFactoryType = typeof(ObservationTypeSelectionFactory) 
        )]
        [Required]
        public string Observation { get; set; } = string.Empty;


        [CriterionPropertyEditor(
            Order = 30,
            SelectionFactoryType = typeof(NumberComparisonSelectionFactory)
        )]

        [Required]
        public string Comparison { get; set; } = string.Empty;

        [Required]

        [CriterionPropertyEditor(Order = 30)]

        [Display(Name = "Value")]
        public int ObservationValue { get; set; }
    }
}