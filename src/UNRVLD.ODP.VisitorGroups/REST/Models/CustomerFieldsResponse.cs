#if NET5_0_OR_GREATER
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.REST.Models
{
    public class CustomerFieldsResponse
    {
        public List<Field> fields { get; set; }
    }
}
#endif