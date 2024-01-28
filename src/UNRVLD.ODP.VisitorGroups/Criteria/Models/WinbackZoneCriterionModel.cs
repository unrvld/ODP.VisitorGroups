using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public class WinbackZoneCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return base.ShallowCopy(); }

        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(WinbackZoneSelectionFactory)
        )]


        [Required]
        public string WinbackZone { get; set; }
    }
}