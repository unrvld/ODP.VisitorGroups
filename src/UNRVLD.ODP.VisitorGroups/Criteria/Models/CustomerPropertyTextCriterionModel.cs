using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class CustomerPropertyTextCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(CustomerPropertyTextSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(CustomerPropertyTextSelectionFactory))]
#endif
        [Required]
        [Display(Name = "Customer Property (number)")]
        public string PropertyName { get; set; }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(TextComparisonSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(TextComparisonSelectionFactory))]
#endif
        [Required]
        public string Comparison { get; set; }

        [Required]
#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(Order = 30)]
#endif
        [Display(Name = "Value")]
        public string PropertyValue { get; set; }
    }
}
