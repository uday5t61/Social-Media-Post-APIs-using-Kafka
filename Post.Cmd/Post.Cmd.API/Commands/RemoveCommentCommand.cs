using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class RemoveCommentCommand : BaseCommand
    {
        public required Guid CommentId { get; set; }
        public required string Username { get; set; }
    }
}