using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;


using System;
using System.Collections.Generic;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Options;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class OdpInstanceSelectionFactory : ISelectionFactory
    {
        private readonly OdpVisitorGroupOptions options;
        public OdpInstanceSelectionFactory()
        {
            
            options = ServiceLocator.Current.GetInstance<IOptions<OdpVisitorGroupOptions>>().Value;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            var selectItems = new List<SelectListItem>();

            try
            {
                foreach (var endPoint in options.OdpEndpoints)
                {
                    selectItems.Add(new SelectListItem() { Text = endPoint.Name, Value = endPoint.Name });
                }

                return selectItems;
            }
            catch
            {
                return [];
            }
        }
    }
}
