namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Edge<T>
    {
        public T Node { get;set;} = default!;
        public string Cursor { get;set;} = string.Empty;
    }
}
