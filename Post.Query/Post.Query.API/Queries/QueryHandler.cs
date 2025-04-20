using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Repositories;
using Post.Query.Domain.Repositories;

namespace Post.Query.API.Queries
{
    public class QueryHandler(IPostRepository postRepository) : IQueryHandler
    {        
        public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
        {
            return await postRepository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await postRepository.GetByIdAsync(query.Id);
            return [post];
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query)
        {
            return await postRepository.ListWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            return await postRepository.ListByAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
        {
            return await postRepository.ListWithLikeAsync(query.NumberOfLikes);
        }
    }
}