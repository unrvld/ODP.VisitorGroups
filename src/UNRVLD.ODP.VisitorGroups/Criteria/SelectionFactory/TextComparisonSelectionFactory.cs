using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class TextComparisonSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items =
            [
                new SelectListItem() { Text = "Is", Value = "Is" },
                new SelectListItem() { Text = "Starts with", Value = "StartsWith" },
                new SelectListItem() { Text = "Contains", Value = "Contains" },
                new SelectListItem() { Text = "Ends with", Value = "EndsWith" },
                new SelectListItem() { Text = "Has a value", Value = "HasValue" },
                new SelectListItem() { Text = "Has no value", Value = "HasNoValue" },
            ];
            return items;
        }
    }
}