using System.Collections.Generic;
using System.Threading.Tasks;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public interface IGraphQLClient
    {
        Task<T> Query<T>(string query) where T : class;
    }
}