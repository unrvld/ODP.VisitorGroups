using System.ComponentModel.DataAnnotations;
using EPiServer.Data.Dynamic;
using EPiServer.Personalization.VisitorGroups;
using UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class OrderLikelihoodCriterionModel : CriterionModelBase
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
            SelectionFactoryType = typeof(OrderLikelihoodSelectionFactory)
        )]

        [Required]
        public string OrderLikelihood { get; set; } = string.Empty;
    }
}