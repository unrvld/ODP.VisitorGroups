using Audience = UNRVLD.ODP.VisitorGroups.GraphQL.Models.Audience;
using System.Threading.Tasks;
using EPiServer.Framework.Cache;

namespace UNRVLD.ODP.VisitorGroups.Criteria.Models
{
    /// <summary>
    /// Used to populate the cache for audience size(s) and meant for background operations
    /// </summary>
    public interface IAudienceSizeCachePopulator
    {
        Task PopulateEntireCache(bool ForceRefresh);
        void PopulateCacheItem(Audience Audience);
    }
}