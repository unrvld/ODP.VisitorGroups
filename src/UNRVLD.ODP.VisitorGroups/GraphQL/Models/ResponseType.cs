namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public abstract class ResponseType<T>
    {
        public abstract Edges<T> Response { get; set; }
    }


}
