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