using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
#if NET5_0_OR_GREATER

#elif NET461_OR_GREATER
//using EPiServer.Personalization.VisitorGroups;
//using EPiServer.Web.Mvc.VisitorGroups;
#endif

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class ObservationCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0_OR_GREATER
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(ObservationTypeSelectionFactory) 
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(ObservationTypeSelectionFactory))]
#endif
        [Required]
        public string Observation { get; set; }

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
        public int ObservationValue { get; set; }
    }
}