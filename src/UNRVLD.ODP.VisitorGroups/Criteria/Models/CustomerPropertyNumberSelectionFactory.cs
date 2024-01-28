﻿using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using UNRVLD.ODP.VisitorGroups.REST;
using UNRVLD.ODP.VisitorGroups.REST.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class CustomerPropertyNumberSelectionFactory : ISelectionFactory
    {
        private readonly ICustomerPropertyListRetriever _customerPropertyListRetriever;

        public CustomerPropertyNumberSelectionFactory()
        {
            _customerPropertyListRetriever = ServiceLocator.Current.GetInstance<ICustomerPropertyListRetriever>();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            IList<SelectListItem> items = new List<SelectListItem>();

            var customerFields = _customerPropertyListRetriever.GetCustomerProperties() ?? Enumerable.Empty<Field>();
            foreach (var customerField in customerFields)
            {
                if (customerField.type == "number")
                {
                    items.Add(new SelectListItem() { Text = customerField.display_name, Value = customerField.name });
                }
            }

            return items.OrderBy(x => x.Text).ToList();
        }
    }
}