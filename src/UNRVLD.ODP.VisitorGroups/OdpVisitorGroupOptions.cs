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
    ///            "EndPoint": "https:///api.zaius.com/v3/graphql",
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
            EndPoint = "https://api.zaius.com/v3/graphql";
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
        /// API endpoint URL, can vary by region ODP is deployed into
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// The private API key to access the ODP Api
        /// </summary>
        public string PrivateApiKey { get; set; }

        public bool IsConfigured => !(string.IsNullOrEmpty(OdpCookieName) ||
                                    string.IsNullOrEmpty(EndPoint) || 
                                    string.IsNullOrEmpty(PrivateApiKey));
    }
}
