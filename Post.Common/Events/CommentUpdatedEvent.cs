using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        protected CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {
        }
        public required Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
        public DateTime EditDate { get; set; }
    }
}