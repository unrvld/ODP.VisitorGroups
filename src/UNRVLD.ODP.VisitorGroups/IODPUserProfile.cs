#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Http;
#elif NET461_OR_GREATER
using System.Web;
#endif

namespace UNRVLD.ODP.VisitorGroups
{
    public interface IODPUserProfile
    {
#if NET5_0_OR_GREATER
        string GetDeviceId(HttpContext httpContext);
#elif NET461_OR_GREATER
        string GetDeviceId(HttpContextBase httpContext);
#endif
    }
}