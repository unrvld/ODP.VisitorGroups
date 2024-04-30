using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class OrderLikelihoodSelectionFactory : ISelectionFactory
    {
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Unlikely", Value = "Unlikely" });
            items.Add(new SelectListItem() { Text = "Likely", Value = "Likely" });
            items.Add(new SelectListItem() { Text = "Very Likely", Value = "VeryLikely" });
            items.Add(new SelectListItem() { Text = "Extremely Likely", Value = "ExtremelyLikely" });

            return items;
        }
    }
}