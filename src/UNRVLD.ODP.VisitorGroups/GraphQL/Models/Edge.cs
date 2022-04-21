namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Edge<T>
    {
        public T Node { get;set;}
        public string Cursor { get;set;}
    }
}
