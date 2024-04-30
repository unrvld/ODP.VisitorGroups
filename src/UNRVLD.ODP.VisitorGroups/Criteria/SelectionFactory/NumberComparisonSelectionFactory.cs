using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;


using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class NumberComparisonSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Less Than", Value = "LessThan" });
            items.Add(new SelectListItem() { Text = "Equal To", Value = "EqualTo" });
            items.Add(new SelectListItem() { Text = "Greater Than", Value = "GreaterThan" });
            return items;
        }
    }
}