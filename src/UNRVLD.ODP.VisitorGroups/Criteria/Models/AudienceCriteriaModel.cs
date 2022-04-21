using EPiServer.Personalization.VisitorGroups;

#if NET5_0
#elif NET461_OR_GREATER
#endif

using System.ComponentModel.DataAnnotations;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class AudienceCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0
        [CriterionPropertyEditor(
               AfterTextTranslationKey = "/path/to/xml",
               LabelTranslationKey = "",
               Order = 1,
               SelectionFactoryType = typeof(AudienciesSelectionFactory)
            )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(AudienciesSelectionFactory))]
#endif
        [Required]

        public string Audience { get; set; }
    }
}
