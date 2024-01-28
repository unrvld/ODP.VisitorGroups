using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class ObservationCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }


        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(ObservationTypeSelectionFactory) 
        )]
        [Required]
        public string Observation { get; set; }


        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(NumberComparisonSelectionFactory)
        )]

        [Required]
        public string Comparison { get; set; }

        [Required]

        [CriterionPropertyEditor(Order = 30)]

        [Display(Name = "Value")]
        public int ObservationValue { get; set; }
    }
}