using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class EditMessageCommand : BaseCommand
    {
        public required string Message { get; set; }
    }
}