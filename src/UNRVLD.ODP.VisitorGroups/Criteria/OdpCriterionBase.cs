using System.Security.Principal;
using EPiServer.Personalization.VisitorGroups;

using Microsoft.AspNetCore.Http;



namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    /// <summary>
    /// An abstract class for a visitor group criterion using strongly typed settings and automatically generated user interface
    /// </summary>
    /// <typeparam name="T">The type of model/settings to operate on.</typeparam>
    public abstract class OdpCriterionBase<T> : CriterionBase<T>
        where T : class, ICriterionModel, new()
    {
        protected IODPUserProfile OdpUserProfile;

        protected OdpCriterionBase(IODPUserProfile odpUserProfile)
        {
            OdpUserProfile = odpUserProfile;
        }
        public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
        {
            var vuidValue = OdpUserProfile.GetDeviceId(httpContext);
            return this.IsMatchInner(principal, vuidValue);
        }


        protected abstract bool IsMatchInner(IPrincipal principal, string vuidValue);
    }
}
