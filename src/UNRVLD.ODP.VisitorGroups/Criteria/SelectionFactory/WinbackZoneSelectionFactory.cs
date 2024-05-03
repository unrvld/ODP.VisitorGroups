using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;


using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class WinbackZoneSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items =
            [
                new SelectListItem() { Text = "Engaged", Value = "Engaged" },
                new SelectListItem() { Text = "Winback", Value = "Winback" },
                new SelectListItem() { Text = "Churned", Value = "Churned" },
            ];
            return items;
        }
    }
}