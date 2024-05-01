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
            IList<SelectListItem> items =
            [
                new SelectListItem() { Text = "Less Than", Value = "LessThan" },
                new SelectListItem() { Text = "Equal To", Value = "EqualTo" },
                new SelectListItem() { Text = "Not Equal To", Value = "NotEqualTo" },
                new SelectListItem() { Text = "Greater Than", Value = "GreaterThan" },
            ];
            
            return items;
        }
    }
}