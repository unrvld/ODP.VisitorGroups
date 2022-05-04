using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET461_OR_GREATER
using System.Web.Mvc;
#endif

using System;
using System.Collections.Generic;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
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