using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CQRS.Core.Queries;

namespace Post.Query.API.Queries
{
    public class FindPostByAuthorQuery : BaseQuery
    {
        public required string Author { get; set; }
    }
}