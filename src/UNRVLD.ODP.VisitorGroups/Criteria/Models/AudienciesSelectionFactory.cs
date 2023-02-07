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
using UNRVLD.ODP.VisitorGroups.GraphQL.Models.AudienceCount;
using EPiServer.Framework.Cache;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    public class AudienciesSelectionFactory : ISelectionFactory
    {
        private readonly IGraphQLClient client;
        private readonly ISynchronizedObjectInstanceCache cache;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";
#if NET5_0_OR_GREATER
        private readonly IServiceScopeFactory serviceScopeFactory;
#endif

        public AudienciesSelectionFactory()
        {
            client = ServiceLocator.Current.GetInstance<IGraphQLClient>();
            cache = ServiceLocator.Current.GetInstance<ISynchronizedObjectInstanceCache>();
#if NET5_0_OR_GREATER
            serviceScopeFactory = ServiceLocator.Current.GetInstance<IServiceScopeFactory>();
#endif
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
                var cachePopulationRequested = false;
                var result = client.Query<AudiencesResponse>(query).Result;
                var orderedResult = result.Items.OrderBy(x => x.Description);

                selectItems = new List<SelectListItem>();
                foreach (var audience in orderedResult)
                {
                    var cacheResult = cache.Get(cacheKey + audience.Name);
                    if (cacheResult != null)
                    {
                        selectItems.Add(new SelectListItem() { Text = audience.Description + GetCountEstimateString((AudienceCount)cacheResult), Value = audience.Name });
                    }
                    else
                    {
                        selectItems.Add(new SelectListItem() { Text = audience.Description + " (Calculating segment size...)", Value = audience.Name });
                        if (cachePopulationRequested == false)
                        {
                            cachePopulationRequested = true;

#if NET5_0_OR_GREATER
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    using var scope = serviceScopeFactory.CreateScope();
                                    var cachePopulator = scope.ServiceProvider.GetRequiredService<IAudienceSizeCachePopulator>();
                                    await cachePopulator.PopulateEntireCache(false);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            });
#elif NET461_OR_GREATER
                            try
                            {
                                var cachePopulator = ServiceLocator.Current.GetInstance<IAudienceSizeCachePopulator>();
                                HostingEnvironment.QueueBackgroundWorkItem(c => cachePopulator.PopulateEntireCache(false));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
#endif
                        }
                    }
                }

                return selectItems;
            }
            catch 
            {
                return new List<SelectListItem>();
            }
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
