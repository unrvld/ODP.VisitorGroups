using System.Collections.Generic;
using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.REST.Models
{
    public class CustomerFieldsResponse
    {
        public ICollection<Field> fields { get; set; }
    }
}