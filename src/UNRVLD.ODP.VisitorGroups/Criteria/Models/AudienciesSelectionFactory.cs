using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET461_OR_GREATER
using System.Web.Mvc;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using IGraphQLClient = UNRVLD.ODP.VisitorGroups.GraphQL.IGraphQLClient;
using EPiServer.ServiceLocation;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class AudienciesSelectionFactory : ISelectionFactory
    {
        private readonly IGraphQLClient client;

        public AudienciesSelectionFactory()
        {
            this.client = ServiceLocator.Current.GetInstance<IGraphQLClient>();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            var query = @"query MyQuery {
                          audiences {
                            edges {
                              node {
                                description
                                name
                              }
                            }
                          }
                        }";

            var result = client.Query<AudiencesResponse>(query).Result;

            return result.Items.Select(a => new SelectListItem { Text = a.Description, Value = a.Name}).OrderBy(x => x.Text);
        }
    }
}
