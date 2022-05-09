using EPiServer.ServiceLocation;

namespace UNRVLD.ODP
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
    ///            "BaseEndPoint": "https:///api.zaius.com",
    ///            "PrivateApiKey": "key-lives-here"
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
            BaseEndPoint = "https://api.zaius.com";
            SchemaCacheTimeoutSeconds = 86400;
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
        public string PrivateApiKey { get; set; }
        /// <summary>
        /// API endpoint base, can vary by region ODP is deployed into
        /// </summary>
        public string BaseEndPoint { get; set; }
        /// <summary>
        /// The length of time to cache schema responses for, can be relatively long lived and defaults to 24 hours
        /// </summary>
        public int SchemaCacheTimeoutSeconds { get; set; }

        public bool IsConfigured => !(string.IsNullOrEmpty(OdpCookieName) ||
                                    string.IsNullOrEmpty(BaseEndPoint) || 
                                    string.IsNullOrEmpty(PrivateApiKey));
    }
}
