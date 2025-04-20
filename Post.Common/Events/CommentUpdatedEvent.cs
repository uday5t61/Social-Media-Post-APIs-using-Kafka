using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {
        }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public required Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
        public DateTime EditDate { get; set; }
    }
}