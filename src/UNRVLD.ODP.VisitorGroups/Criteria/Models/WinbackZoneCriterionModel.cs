using System.ComponentModel.DataAnnotations;
using EPiServer.Data.Dynamic;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class WinbackZoneCriterionModel : CriterionModelBase
    {
        public override ICriterionModel Copy() { return ShallowCopy(); }

        [CriterionPropertyEditor(
            Order = 10,
            SelectionFactoryType = typeof(OdpInstanceSelectionFactory)
        )]

        [Required]
        public string InstanceName { get; set; } = string.Empty;

        [CriterionPropertyEditor(
            Order = 20,
            SelectionFactoryType = typeof(WinbackZoneSelectionFactory)
        )]


        [Required]
        public string WinbackZone { get; set; } = string.Empty;
    }
}