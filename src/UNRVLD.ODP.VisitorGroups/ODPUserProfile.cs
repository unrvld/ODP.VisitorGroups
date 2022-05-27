using System;
#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
using EPiServer.ServiceLocation;
#endif

namespace UNRVLD.ODP.VisitorGroups
{
    public class ODPUserProfile : IODPUserProfile
    {
        private readonly OdpVisitorGroupOptions _optionValues;
        public ODPUserProfile(OdpVisitorGroupOptions optionValues)
        {
            _optionValues = optionValues;
        }

#if NET5_0_OR_GREATER
        public string GetDeviceId(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                var vuidValue = httpContext.Request.Cookies[_optionValues.OdpCookieName];
                return GetVuidValueInternal(vuidValue);
            }

            return null;
        }
#elif NET461_OR_GREATER
        public string GetDeviceId(HttpContextBase httpContext)
        {
            if (httpContext != null)
            {
                var vuidValue = httpContext.Request.Cookies[_optionValues.OdpCookieName]?.Value;
                return GetVuidValueInternal(vuidValue);
            }

            return null;
        }
#endif
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