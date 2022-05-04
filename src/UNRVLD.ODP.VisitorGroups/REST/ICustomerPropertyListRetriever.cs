#if NET5_0
using System.Collections.Generic;
using UNRVLD.ODP.VisitorGroups.REST.Models;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public interface ICustomerPropertyListRetriever
    {
        IEnumerable<Field> GetCustomerProperties();
    }
}
#endif