using EPiServer.Personalization.VisitorGroups;

using System.ComponentModel.DataAnnotations;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class AudienceCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

        [CriterionPropertyEditor(
               AfterTextTranslationKey = "/path/to/xml",
               LabelTranslationKey = "",
               Order = 1,
               SelectionFactoryType = typeof(AudienciesSelectionFactory)
            )]
        [Required]

        public string Audience { get; set; } = string.Empty;
    }
}
