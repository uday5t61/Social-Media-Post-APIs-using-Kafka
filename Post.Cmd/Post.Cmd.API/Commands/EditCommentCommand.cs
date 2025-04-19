using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class EditCommentCommand : BaseCommand
    {
        public Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
    }
}