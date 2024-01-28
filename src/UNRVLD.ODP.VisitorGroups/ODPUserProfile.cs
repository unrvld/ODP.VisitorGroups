using System;

using Microsoft.AspNetCore.Http;


namespace UNRVLD.ODP.VisitorGroups
{
    public class ODPUserProfile : IODPUserProfile
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        public ODPUserProfile(OdpVisitorGroupOptions optionValues)
        {
            _optionValues = optionValues;
        }
        public string GetDeviceId(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                var vuidValue = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                return GetVuidValueInternal(vuidValue);
            }

            return null;
        }

        private string GetVuidValueInternal(string vuidValue)
        {
            if (!string.IsNullOrWhiteSpace(vuidValue) && vuidValue.Length > 35)
            {
                return vuidValue.Substring(0, 36).Replace("-", string.Empty);
            }

            return null;
        }
    }
}