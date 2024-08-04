using Microsoft.AspNetCore.Http;
using UNRVLD.ODP.VisitorGroups.Configuration;


namespace UNRVLD.ODP.VisitorGroups
{
    public class ODPUserProfile(OdpVisitorGroupOptions optionValues) : IODPUserProfile
    {
        private readonly OdpVisitorGroupOptions _optionValues = optionValues;

        public string? GetDeviceId(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                var vuidValue = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                if (string.IsNullOrEmpty(vuidValue))
                {
                    vuidValue = httpContext.Request.Query["vuid"];
                }
                return GetVuidValueInternal(vuidValue);
            }

            return null;
        }

        private string? GetVuidValueInternal(string? vuidValue)
        {
            if (!string.IsNullOrWhiteSpace(vuidValue) && vuidValue.Length > 35)
            {
                return vuidValue[..36].Replace("-", string.Empty);
            }

            return null;
        }
    }
}
