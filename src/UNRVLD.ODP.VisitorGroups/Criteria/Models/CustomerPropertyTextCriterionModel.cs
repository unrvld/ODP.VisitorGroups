using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class CustomerPropertyTextCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(CustomerPropertyTextSelectionFactory)
        )]
        [Required]
        [Display(Name = "Customer Property (number)")]
        public string PropertyName { get; set; } = string.Empty;

        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(TextComparisonSelectionFactory)
        )]
        [Required]
        public string Comparison { get; set; } = string.Empty;

        [Required]
        [CriterionPropertyEditor(Order = 30)]
        [Display(Name = "Value")]
        public string PropertyValue { get; set; } = string.Empty;
    }
}
