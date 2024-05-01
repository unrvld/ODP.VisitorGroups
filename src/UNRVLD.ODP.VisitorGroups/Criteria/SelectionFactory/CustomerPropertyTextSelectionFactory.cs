using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;

using UNRVLD.ODP.VisitorGroups.REST;
using UNRVLD.ODP.VisitorGroups.REST.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using UNRVLD.ODP.VisitorGroups.Configuration;
using Microsoft.Extensions.Options;


namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class CustomerPropertyTextSelectionFactory : ISelectionFactory
    {
        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;
        private readonly OdpVisitorGroupOptions _options;

        public CustomerPropertyTextSelectionFactory()
        {
            _customerPropertyListRetriever = ServiceLocator.Current.GetInstance<ICustomerPropertyListRetriever>();
            _options = ServiceLocator.Current.GetInstance<IOptions<OdpVisitorGroupOptions>>().Value;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            List<Field> customerFields = [];

            foreach (var endpoint in _options.OdpEndpoints)
            {
                 customerFields.AddRange(_customerPropertyListRetriever.GetCustomerProperties(endpoint.Name) ?? []);
            }

            foreach (var customerField in customerFields)
            {
                if (customerField.Type == "string")
                {
                    items.Add(new SelectListItem() { Text = customerField.DisplayName, Value = customerField.Name });
                }
            }

            return [.. items.OrderBy(x => x.Text)];
        }
    }
}
