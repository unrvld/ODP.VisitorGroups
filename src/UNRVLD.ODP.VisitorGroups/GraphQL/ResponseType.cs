using System.Collections.Generic;
using System.Linq;

namespace UNRVLD.ODP.VisitorGroups.GraphQL
{
    public abstract class ResponseType<T> 
    {
        public abstract Edges<T> Response { get; set; }

        public virtual IEnumerable<T> Items => this.Response?.EdgeItems?.Select(e => e.Node) ?? Enumerable.Empty<T>();
    }
}
