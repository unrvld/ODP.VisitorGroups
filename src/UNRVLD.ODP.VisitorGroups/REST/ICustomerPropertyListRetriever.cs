#if NET5_0
using System.Collections.Generic;
using System.Threading.Tasks;
using UNRVLD.ODP.VisitorGroups.REST.Models;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public interface ICustomerPropertyListRetriever
    {
        Task<IEnumerable<Field>> GetCustomerPropertiesAsync();
    }
}
#endif