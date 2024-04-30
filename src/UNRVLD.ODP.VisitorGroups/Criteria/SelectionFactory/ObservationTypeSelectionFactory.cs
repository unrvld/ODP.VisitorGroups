using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;


using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class ObservationTypeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Total Revenue", Value = "TotalRevenue" });
            items.Add(new SelectListItem() { Text = "Order Count", Value = "OrderCount" });
            items.Add(new SelectListItem() { Text = "Average Order Revenue", Value = "AverageOrderRevenue" });
            return items;
        }
    }
}