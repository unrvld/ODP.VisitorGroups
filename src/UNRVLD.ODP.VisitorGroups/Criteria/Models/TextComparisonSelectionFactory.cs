using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class TextComparisonSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Is", Value = "Is" });
            items.Add(new SelectListItem() { Text = "Starts with", Value = "StartsWith" });
            items.Add(new SelectListItem() { Text = "Contains", Value = "Contains" });
            items.Add(new SelectListItem() { Text = "Ends with", Value = "EndsWith" });
            items.Add(new SelectListItem() { Text = "Has a value", Value = "HasValue" });
            items.Add(new SelectListItem() { Text = "Has no value", Value = "HasNoValue" });
            return items;
        }
    }
}