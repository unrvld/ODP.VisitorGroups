using UNRVLD.ODP.VisitorGroups.GraphQL.Models;

namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    public interface ICustomerDataRetriever
    {
        Customer? GetCustomerInfo(string vuidValue, string OdpIdQueryField, string? endpointKey = null);

        bool IsInAudience(string vuidValue, string OdpIdQueryField, string audience, string? endpointKey = null);
    }
}