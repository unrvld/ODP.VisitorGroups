using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;

using UNRVLD.ODP.VisitorGroups.REST;
using UNRVLD.ODP.VisitorGroups.REST.Models;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET461_OR_GREATER
using System.Web.Mvc;
#endif

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class CustomerPropertyTextSelectionFactory : ISelectionFactory
    {
        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;

        public CustomerPropertyTextSelectionFactory()
        {
            _customerPropertyListRetriever = ServiceLocator.Current.GetInstance<ICustomerPropertyListRetriever>();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();

            var customerFields = _customerPropertyListRetriever.GetCustomerProperties() ?? Enumerable.Empty<Field>();
            foreach (var customerField in customerFields)
            {
                if (customerField.type == "string")
                {
                    items.Add(new SelectListItem() { Text = customerField.display_name, Value = customerField.name });
                }
            }

            return items.OrderBy(x => x.Text).ToList();
        }
    }
}
