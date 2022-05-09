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
        private Lazy<string> _deviceId;

#if NET5_0_OR_GREATER
        private readonly HttpContext _httpContextBase;
#elif NET461_OR_GREATER

        private readonly HttpContextBase _httpContextBase;
#endif

      
#if NET5_0_OR_GREATER
        public ODPUserProfile(OdpVisitorGroupOptions optionValues, HttpContext httpContextBase)
#elif NET461_OR_GREATER

        public ODPUserProfile(OdpVisitorGroupOptions optionValues, HttpContextBase httpContextBase)
#endif
        {
            _optionValues = optionValues;
            _httpContextBase = httpContextBase;

            _deviceId = new Lazy<string>(() => {

#if NET5_0_OR_GREATER
                var vuidValue = _httpContextBase.Request.Cookies[_optionValues.OdpCookieName];
#elif NET461_OR_GREATER
                var vuidValue = _httpContextBase.Request.Cookies[_optionValues.OdpCookieName]?.Value;
#endif

                if (!string.IsNullOrWhiteSpace(vuidValue))
                {
                    vuidValue = vuidValue.Substring(0, 36).Replace("-", "");
                }

                return vuidValue;
            });
        }

        public string DeviceId { get { return _deviceId.Value; } }
    }
}