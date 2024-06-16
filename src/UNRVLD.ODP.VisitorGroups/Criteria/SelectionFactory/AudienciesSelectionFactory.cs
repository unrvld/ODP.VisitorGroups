﻿using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using EPiServer.Framework.Cache;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using Microsoft.Extensions.Options;
using UNRVLD.ODP.VisitorGroups.Configuration;
using Microsoft.Extensions.Logging;

namespace UNRVLD.ODP.VisitorGroups.Criteria.SelectionFactory
{
    public class AudienciesSelectionFactory : ISelectionFactory
    {
        private readonly IGraphQLClientFactory clientFactory;
        private readonly ISynchronizedObjectInstanceCache cache;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";

        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly OdpVisitorGroupOptions options;

        private readonly IPrefixer prefixer;
        private readonly ILogger<AudienciesSelectionFactory> _logger;

        public AudienciesSelectionFactory()
        {
            clientFactory = ServiceLocator.Current.GetInstance<IGraphQLClientFactory>();

            options = ServiceLocator.Current.GetInstance<IOptions<OdpVisitorGroupOptions>>().Value;
            cache = ServiceLocator.Current.GetInstance<ISynchronizedObjectInstanceCache>();
            serviceScopeFactory = ServiceLocator.Current.GetInstance<IServiceScopeFactory>();
            prefixer = ServiceLocator.Current.GetInstance<IPrefixer>();  

            _logger = ServiceLocator.Current.GetInstance<ILoggerFactory>().CreateLogger<AudienciesSelectionFactory>();

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
                foreach (var endPoint in options.OdpEndpoints)
                {
                    var client = clientFactory.Get(endPoint);
                    var result = client.Query<AudiencesResponse>(query).Result;

                    if (result is not null)
                        selectItems.AddRange(GetAudienceDetails(options.HasMultipleEndpoints, endPoint, result));
                }

                return selectItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching audience details");
                return [];
            }
        }

        private List<SelectListItem> GetAudienceDetails(bool hasMultipleEndpoints, OdpEndpoint endPoint, AudiencesResponse result)
        {
            var selectItems = new List<SelectListItem>();

            var cachePopulationRequested = false;

            var orderedResult = result.Items.OrderBy(x => x.Description);

            selectItems = [];

            foreach (var audience in orderedResult)
            {
                var cacheResult = cache.Get($"{cacheKey}-{endPoint.Name}-{audience.Name}");

                var textPrefix = hasMultipleEndpoints ? prefixer.Prefix(audience.Description, endPoint.Name) : audience.Description;
                var value = hasMultipleEndpoints ?prefixer.Prefix(audience.Name, endPoint.Name) : audience.Name;

                if (cacheResult != null)
                {
                    selectItems.Add(new SelectListItem() { Text = $"{textPrefix} {GetCountEstimateString((AudienceCount)cacheResult)}", Value = value });
                }
                else
                {
                    selectItems.Add(new SelectListItem() { Text = $"{textPrefix} (Calculating segment size...)", Value = value });
                    if (cachePopulationRequested == false)
                    {
                        cachePopulationRequested = true;

                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                using var scope = serviceScopeFactory.CreateScope();
                                var cachePopulator = scope.ServiceProvider.GetRequiredService<IAudienceSizeCachePopulator>();
                                await cachePopulator.PopulateEntireCache(endPoint, false);
                            }
                            catch (Exception e)
                            {
                                 _logger.LogError(e, "Error getting audience details.");
                            }
                        });
                    }
                }
            }

            return selectItems;
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
