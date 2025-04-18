using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class CommentRemovedEvent : BaseEvent
    {
        protected CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
        {
        }
        public required Guid CommentId { get; set; }
    }
}