using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class CustomerPropertyNumberCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(CustomerPropertyNumberSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(CustomerPropertyNumberSelectionFactory))]
#endif
        [Required]
        [Display(Name = "Customer Property (number)")]
        public string PropertyName { get; set; }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(NumberComparisonSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(NumberComparisonSelectionFactory))]
#endif
        [Required]
        public string Comparison { get; set; }

        [Required]
#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(Order = 30)]
#endif
        [Display(Name = "Value")]
        public int PropertyValue { get; set; }
    }
}