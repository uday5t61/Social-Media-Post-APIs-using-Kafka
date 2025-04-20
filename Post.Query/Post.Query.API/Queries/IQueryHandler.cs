using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.API.Queries
{
    public interface IQueryHandler
    {
        Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query);
    }
}