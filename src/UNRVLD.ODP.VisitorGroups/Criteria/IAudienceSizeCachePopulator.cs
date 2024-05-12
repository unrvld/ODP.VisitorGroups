using System.Threading.Tasks;
using UNRVLD.ODP.VisitorGroups.Configuration;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    /// <summary>
    /// Used to populate the cache for audience size(s) and meant for background operations
    /// </summary>
    public interface IAudienceSizeCachePopulator
    {
        Task PopulateEntireCache(OdpEndpoint endPoint, bool ForceRefresh);
        //void PopulateCacheItem(Audience Audience);
    }
}