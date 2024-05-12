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
            IList<SelectListItem> items =
            [
                new SelectListItem() { Text = "Unlikely", Value = "Unlikely" },
                new SelectListItem() { Text = "Likely", Value = "Likely" },
                new SelectListItem() { Text = "Very Likely", Value = "VeryLikely" },
                new SelectListItem() { Text = "Extremely Likely", Value = "ExtremelyLikely" },
            ];

            return items;
        }
    }
}