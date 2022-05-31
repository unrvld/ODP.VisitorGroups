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
using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models.AudienceCount;
using Audience = UNRVLD.ODP.VisitorGroups.GraphQL.Models.Audience;

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

            var selectItems = new List<SelectListItem>();

            try
            {
                var result = client.Query<AudiencesResponse>(query).Result;
                var orderedResult = result.Items.OrderBy(x => x.Description);

                var allCountsQuery = GetAllCountsQuery(orderedResult.ToList());
                var allCountsResult = client.Query<dynamic>(allCountsQuery).Result;


                foreach (var audience in orderedResult)
                {
                    var countObject = JsonConvert.DeserializeObject<AudienceCount>(allCountsResult[audience.Name].ToString());
                    selectItems.Add(new SelectListItem() {Text = audience.Description + GetCountEstimateString(countObject), Value = audience.Name});
                }

                return selectItems;
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        private string GetAllCountsQuery(List<Audience> allAudiences)
        {
            var countQuery = $@"query MyQuery {{";
            foreach (var audience in allAudiences)
            {
                countQuery += $@"{audience.Name}: audience(name: ""{audience.Name}"") {{
                                 population_estimate(percent_error: 10) {{
                                   estimated_lower_bound
                                   estimated_upper_bound
                                 }}
                               }}
                               ";
            }
            countQuery += $@"}}";

            return countQuery;
        }

        private string GetCountEstimateString(AudienceCount audienceCount)
        {
            if (audienceCount == null ||
                audienceCount.PopulationEstimate == null)
            {
                return string.Empty;
            }

            if (audienceCount.PopulationEstimate.EstimatedLowerBound == 0)
            {
                return " (close to 0 visitors)";
            }

            if (audienceCount.PopulationEstimate.EstimatedLowerBound == 1)
            {
                return " (more than 1 visitor)";
            }

            if (audienceCount.PopulationEstimate.EstimatedLowerBound < 100)
            {
                return $" (more than {audienceCount.PopulationEstimate.EstimatedLowerBound} visitors)";
            }

            int calc = (audienceCount.PopulationEstimate.EstimatedLowerBound +
                        audienceCount.PopulationEstimate.EstimatedUpperBound) / 2;
            return $" (about {calc} visitors)";

        }
    }
}
