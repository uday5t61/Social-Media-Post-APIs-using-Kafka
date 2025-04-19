using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler(IPostRepository postRepository,ICommentRepository commentRepository) : IEventHandler
    {
        public async Task On(PostCreatedEvent @event)
        {
            var post = new PostEntity
            {
                Author = @event.Author,
                PostId = @event.Id,
                Message = @event.Message,
                DatePosted = @event.DatePosted
            };
            await postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            var post = await postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Message = @event.Message;
            await postRepository.UpdateAsync(post);
        }

        public async Task On(PostLikedEvent @event)
        {
            var post = await postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Likes++;
            await postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedEvent @event)
        {
            var comment = new CommentEntity
            {
                CommentId = @event.CommentId,
                Comment = @event.Comment,
                CommentData = @event.CommentDate,
                IsEdited = false,
                Username = @event.Username,
                PostId = @event.Id
            };

            await commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            var comment = await commentRepository.GetByIdAsync(@event.Id);

            if (comment == null) return;

            comment.Comment = @event.Comment;
            comment.Username = @event.Username;
            comment.IsEdited = true;
            comment.CommentData = @event.EditDate;
            await commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemovedEvent @event)
        {           
            await commentRepository.DeleteAsync(@event.Id);
        }

        public async Task On(PostRemovedEvent @event)
        {
            await postRepository.DeleteAsync(@event.Id);
        }
    }
}
