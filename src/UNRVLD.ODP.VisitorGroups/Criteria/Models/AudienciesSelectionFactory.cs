using EPiServer.Personalization.VisitorGroups;

#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET461_OR_GREATER
using System.Web.Mvc;
using System.Web.Hosting;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using IGraphQLClient = UNRVLD.ODP.VisitorGroups.GraphQL.IGraphQLClient;
using EPiServer.ServiceLocation;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using EPiServer.Framework.Cache;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class AudienciesSelectionFactory : ISelectionFactory
    {
        private readonly IGraphQLClient client;
        private readonly ISynchronizedObjectInstanceCache cache;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";

        public AudienciesSelectionFactory()
        {
            client = ServiceLocator.Current.GetInstance<IGraphQLClient>();
            cache = ServiceLocator.Current.GetInstance<ISynchronizedObjectInstanceCache>();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            var query = @"query MyQuery {
                          audiences {
                            edges {
                              node {
                                description
                                name
                                population_estimate(percent_error: 10) {
                                  estimated_lower_bound
                                  estimated_upper_bound
                                }
                              }
                            }
                          }
                        }";

            try
            {
                var result = client.Query<AudiencesResponse>(query).Result;

                return result.Items
                    .OrderBy(x => x.Description)
                    .Select(audience => new SelectListItem { Text = audience.Description + GetCountEstimateString(audience), Value = audience.Name })
                    .ToArray();
            }
            catch
            {
                return Enumerable.Empty<SelectListItem>();
            }
        }

        private string GetCountEstimateString(Audience audience)
        {
            if (audience?.PopulationEstimate == null)
            {
                return string.Empty;
            }

            var populationEstimate = audience.PopulationEstimate;
            if (populationEstimate.EstimatedLowerBound == 0)
            {
                return " (close to 0 visitors)";
            }

            if (populationEstimate.EstimatedLowerBound == 1)
            {
                return " (more than 1 visitor)";
            }

            if (populationEstimate.EstimatedLowerBound < 100)
            {
                return $" (more than {populationEstimate.EstimatedLowerBound} visitors)";
            }

            int calc = (populationEstimate.EstimatedLowerBound +
                        populationEstimate.EstimatedUpperBound) / 2;
            return $" (about {calc} visitors)";

        }
    }
}
