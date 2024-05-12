namespace UNRVLD.ODP.VisitorGroups.Configuration
{
    public class OdpEndpoint
    {

        public OdpEndpoint()
        {
            Name = "Default";
            BaseEndPoint = "https://api.zaius.com";
            PrivateApiKey = "key-lives-here";
        }

        public string Name { get; set; }
        public string BaseEndPoint { get; set; }
        public string PrivateApiKey { get; set; }

        public bool IsConfigured => !(string.IsNullOrEmpty(Name) ||
                                    string.IsNullOrEmpty(BaseEndPoint) ||
                                    string.IsNullOrEmpty(PrivateApiKey));
    }
}
