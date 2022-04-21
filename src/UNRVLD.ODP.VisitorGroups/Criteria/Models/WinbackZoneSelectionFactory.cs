using EPiServer.Personalization.VisitorGroups;

#if NET5_0
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET461_OR_GREATER
using System.Web.Mvc;
#endif

using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class WinbackZoneSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Engaged", Value = "Engaged" });
            items.Add(new SelectListItem() { Text = "Winback", Value = "Winback" });
            items.Add(new SelectListItem() { Text = "Churned", Value = "Churned" });
            return items;
        }
    }
}