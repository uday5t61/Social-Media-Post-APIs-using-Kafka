using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Infrastructure.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = [];

        public bool Active { get => _active; set => _active = value; }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Author = author,
                Message = message,
                Id = id,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;

        }
        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not edit the message of inactive post.");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} can not be empty.");

            }
            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent messageUpdatedEvent)
        {
            _id = messageUpdatedEvent.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not like inactive post.");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void Addcomment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not add the comment to inactive post.");
            }
            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} can not be empty.");
            }
            RaiseEvent(new CommentAddedEvent
            {
                Comment = comment,
                Username = username,
                Id = _id,
                CommentDate = DateTime.Now,
                CommentId = Guid.NewGuid()
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, String comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not edit the comment of inactive post.");
            }
            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} can not be empty.");
            }

            if (_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to edit comment made by another user");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now
            });
        }
        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not remove the comment of inactive post.");
            }
            if (_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to remove comment made by another user");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                CommentId = commentId,
                Id = _id,
            });
        }
        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }
            if (_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to remove post made by another user");
            }

            RaiseEvent(new PostRemovedEvent
            {
                Id = _id,
            });
        }
        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}