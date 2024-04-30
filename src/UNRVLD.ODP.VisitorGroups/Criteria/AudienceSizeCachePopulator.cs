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

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    /// <inheritdoc/>
    public class AudienceSizeCachePopulator : IAudienceSizeCachePopulator
    {
        private readonly IGraphQLClientFactory _clientFactory;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly OdpVisitorGroupOptions _options;
        private string cacheKey = "OdpVisitorGroups_AudienceList_";

        public AudienceSizeCachePopulator(
            IGraphQLClientFactory clientFactory,
            ISynchronizedObjectInstanceCache cache,
            OdpVisitorGroupOptions options)
        {
            _clientFactory = clientFactory;
            _cache = cache;
            _options = options;
        }
        /*
                public void PopulateCacheItem(Audience Audience)
                {
                    try
                    {
                        var countQuery = GetAllCountsQuery([Audience]);
                        var countResult = _client.Query<dynamic>(countQuery).Result;
                        var countObject = JsonConvert.DeserializeObject<AudienceCount>(countResult[Audience.Name].ToString());

                        _cache.Insert(cacheKey + Audience.Name, countObject, new CacheEvictionPolicy(new TimeSpan(0, 0, _options.PopulationEstimateCacheTimeoutSeconds), CacheTimeoutType.Absolute));
                    }
                    catch { }
                }
        */
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
                    var countResult = client.Query<dynamic>(countQuery).Result;

                    var loopCount = 0;
                    foreach (var audienceCount in countResult ?? Enumerable.Empty<dynamic>())
                    {
                        var audience = orderedResult.Skip(skip + loopCount).Take(1).First();
                        var countObject = JsonConvert.DeserializeObject<AudienceCount>(countResult?[audience.Name].ToString());

                        _cache.Insert(
                            $"{cacheKey}-{endPoint.Name}-{audience.Name}",
                            countObject,
                            new CacheEvictionPolicy(new TimeSpan(0, 0, 0, _options.PopulationEstimateCacheTimeoutSeconds), CacheTimeoutType.Absolute));


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
