
using Microsoft.AspNetCore.Http;


namespace UNRVLD.ODP.VisitorGroups
{
    public interface IODPUserProfile
    {
        string? GetDeviceId(HttpContext httpContext);
    }
}