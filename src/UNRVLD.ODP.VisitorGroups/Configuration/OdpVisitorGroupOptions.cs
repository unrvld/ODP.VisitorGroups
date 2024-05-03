using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;

namespace UNRVLD.ODP.VisitorGroups.Configuration
{
    /// <summary>
    /// Options class to set defaults but also allow these to be overridden in code/appsettings as needed. 
    /// </summary>
    /// <example>
    /// Use the following in appsettings.json to configure:
    /// {
    ///    "EPiServer": {
    ///        //Other config
    ///        "OdpVisitorGroupOptions": {
    ///            "OdpCookieName": "vuid",
    ///            "CacheTimeoutSeconds": 10,
    ///            "SchemaCacheTimeoutSeconds": 86400,
    ///            "PopulationEstimateCacheTimeoutSeconds": 4320,
    ///            "OdpEndpoints": [
    ///             {
    ///                 "Name": "US",
    ///                 "BaseEndPoint": "https:///api.zaius.com",
    ///                 "PrivateApiKey": "key-lives-here"
    ///             },
    ///             {
    ///                 "Name": "EU",
    ///                 "BaseEndPoint": "https:///api.zaius.eu",
    ///                 "PrivateApiKey": "key-lives-here"
    ///             }]
    ///        }
    ///    }
    ///}
    /// </example>
    [Options]
    public class OdpVisitorGroupOptions
    {
        public OdpVisitorGroupOptions()
        {
            OdpCookieName = "vuid";
            CacheTimeoutSeconds = 1;
            OdpEndpoints = [];
            SchemaCacheTimeoutSeconds = 86400;
            PopulationEstimateCacheTimeoutSeconds = 4320;
        }
        /// <summary>
        /// The cookie name of the ODP cookie, should nearly always be "vuid"
        /// </summary>
        public string OdpCookieName { get; set; }
        /// <summary>
        /// The length of time to micro-cache responses for, used to avoid multiple API hits in a single request
        /// </summary>
        public int CacheTimeoutSeconds { get; set; }
        /// <summary>
        /// The private API key to access the ODP Api
        /// </summary>
        //public string PrivateApiKey { get; set; }
        /// <summary>
        /// API endpoint base, can vary by region ODP is deployed into
        /// </summary>
        //public string BaseEndPoint { get; set; }
        /// <summary>
        /// The length of time to cache schema responses for, can be relatively long lived and defaults to 24 hours
        /// </summary>
        public int SchemaCacheTimeoutSeconds { get; set; }
        /// <summary>
        /// The length of time to cache the population estimates, defaults to 12 hours
        /// </summary>
        public int PopulationEstimateCacheTimeoutSeconds { get; set; }

        public List<OdpEndpoint> OdpEndpoints { get; set; }

        public bool IsConfigured => !(string.IsNullOrEmpty(OdpCookieName) ||
                                      (OdpEndpoints?.Any(x => !x.IsConfigured) ?? false));

        public bool HasMultipleEndpoints => OdpEndpoints?.Count > 1;

        public OdpEndpoint? GetEndpoint(string? name)
        {
            return name is null ? OdpEndpoints.FirstOrDefault() : OdpEndpoints?.FirstOrDefault(x => x.Name == name);
        }
    }
}
