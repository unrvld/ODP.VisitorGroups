using System;
using System.Collections.Generic;
using System.Linq;
using IGraphQLClient = UNRVLD.ODP.VisitorGroups.GraphQL.IGraphQLClient;
using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models.AudienceCount;
using Audience = UNRVLD.ODP.VisitorGroups.GraphQL.Models.Audience;
using EPiServer.Framework.Cache;
using System.Threading.Tasks;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    /// <inheritdoc/>
    public class AudienceSizeCachePopulator : IAudienceSizeCachePopulator
    {
        private readonly IGraphQLClient _client;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly OdpVisitorGroupOptions _options;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";

        public AudienceSizeCachePopulator(IGraphQLClient client, ISynchronizedObjectInstanceCache cache, OdpVisitorGroupOptions options)
        {
            _client = client;
            _cache = cache;
            _options = options;
        }

        public void PopulateCacheItem(Audience Audience)
        {
            try
            {
                var countQuery = GetAllCountsQuery(new List<Audience> { Audience });
                var countResult = _client.Query<dynamic>(countQuery).Result;
                var countObject = JsonConvert.DeserializeObject<AudienceCount>(countResult[Audience.Name].ToString());

                _cache.Insert(cacheKey + Audience.Name, countObject, new CacheEvictionPolicy(new TimeSpan(0, 0, _options.PopulationEstimateCacheTimeoutSeconds), CacheTimeoutType.Absolute));
            }
            catch { }
        }

        public async Task PopulateEntireCache(bool ForceRefresh)
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
                var result = await _client.Query<AudiencesResponse>(query);
                var orderedResult = result.Items.OrderBy(x => x.Description);

                var skip = 0;
                var pageSize = 10;
                while (orderedResult.Skip(skip).Take(pageSize).ToList().Count > 0)
                {
                    var currentPageOfAudiences = orderedResult.Skip(skip).Take(pageSize).ToList();

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
                    var countResult = _client.Query<dynamic>(countQuery).Result;

                    var loopCount = 0;
                    foreach (var audienceCount in countResult)
                    {
                        var audience = orderedResult.Skip(skip + loopCount).Take(1).First();
                        var countObject = JsonConvert.DeserializeObject<AudienceCount>((countResult[audience.Name].ToString()));

#if NET5_0_OR_GREATER
                        _cache.Insert(
                            cacheKey + audience.Name,
                            countObject,
                            new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _options.PopulationEstimateCacheTimeoutSeconds), CacheTimeoutType.Absolute));
#elif NET461_OR_GREATER
                        // Strange behaviour in .net Framework, where it thinks the Insert method 
                        // doesn't exist so accessing the ObjectInstanceCache directly to resolve
                        _cache.ObjectInstanceCache.Insert(
                            cacheKey + audience.Name,
                            countObject,
                            new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _options.PopulationEstimateCacheTimeoutSeconds), CacheTimeoutType.Absolute));
#endif

                        loopCount++;
                    }

                    skip += pageSize;
                }
            }
            catch (Exception) { }
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
