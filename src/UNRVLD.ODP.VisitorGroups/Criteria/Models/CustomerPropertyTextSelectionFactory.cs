#if NET5_0
using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Rendering;
using UNRVLD.ODP.VisitorGroups.REST;

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

            var customerFields = _customerPropertyListRetriever.GetCustomerProperties();
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
#endif