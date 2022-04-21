using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class WinbackZoneCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

#if NET5_0
        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(WinbackZoneSelectionFactory)
        )]
#elif NET461_OR_GREATER
        [DojoWidget(
              WidgetType = "dijit.form.FilteringSelect",
              SelectionFactoryType = typeof(WinbackZoneSelectionFactory))]
#endif

        [Required]
        public string WinbackZone { get; set; }
    }
}