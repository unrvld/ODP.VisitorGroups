using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using Audience = UNRVLD.ODP.VisitorGroups.GraphQL.Models.Audience;
using EPiServer.Framework.Cache;
using System.Threading.Tasks;
using UNRVLD.ODP.VisitorGroups.GraphQL;
using UNRVLD.ODP.VisitorGroups.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    /// <inheritdoc/>
    public class AudienceSizeCachePopulator : IAudienceSizeCachePopulator
    {
        private readonly IGraphQLClientFactory _clientFactory;
        private readonly ILogger<AudienceSizeCachePopulator> _logger;
        private readonly IMemoryCache _cache;
        private readonly OdpVisitorGroupOptions _options;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";

        public AudienceSizeCachePopulator(
            IGraphQLClientFactory clientFactory,
            ILogger<AudienceSizeCachePopulator> logger,
            IMemoryCache cache,
            OdpVisitorGroupOptions options)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _cache = cache;
            _options = options;
        }

        public async Task PopulateEntireCache(OdpEndpoint endPoint, bool ForceRefresh)
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

            try
            {
                var client = _clientFactory.Get(endPoint);
                if (client == null)
                {
                    return;
                }

                var result = await client.Query<AudiencesResponse>(query);
                var orderedResult = result?.Items.OrderBy(x => x.Description) ?? Enumerable.Empty<Audience>();

                var skip = 0;
                var pageSize = 10;
                
                var currentPageOfAudiences = orderedResult.Skip(skip).Take(pageSize).ToList();
                
                while (currentPageOfAudiences.Count > 0)
                {
                    // Getting the estimate is expensive so skip if we have everything in the cache already (unless we are forcing a refresh)
                    if (ForceRefresh == false)
                    {
                        if (CheckCacheHit(currentPageOfAudiences, pageSize))
                        {
                            skip += pageSize;
                            break;
                        }
                    }

                    var countQuery = GetAllCountsQuery(currentPageOfAudiences);
                    var countResult = client.Query<dynamic>(countQuery).Result;

                    var loopCount = 0;
                    foreach (var audienceCount in countResult ?? Enumerable.Empty<dynamic>())
                    {
                        var audience = orderedResult.Skip(skip + loopCount).Take(1).First();
                        AudienceCount? countObject = JsonConvert.DeserializeObject<AudienceCount>(countResult?[audience.Name].ToString());

                        _cache.Set(
                            $"{cacheKey}-{endPoint.Name}-{audience.Name}",
                            countObject,
                            new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.PopulationEstimateCacheTimeoutSeconds)
                            });


                        loopCount++;
                    }

                    skip += pageSize;

                    currentPageOfAudiences = [.. orderedResult.Skip(skip).Take(pageSize)];
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error populating audience size");
             }
        }

        private bool CheckCacheHit(IList<Audience> CurrentPageOfAudiences, int PageSize)
        {
            var cacheHit = 0;
            foreach (var cacheCheck in CurrentPageOfAudiences)
            {
                var cachedResult = _cache.Get(cacheKey + cacheCheck.Name);
                if (cachedResult != null)
                {
                    cacheHit++;
                }
                else
                {
                    return false;
                }
            }
            if (cacheHit == PageSize)
            {
                return true;
            }

            return false;
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
    }
}
