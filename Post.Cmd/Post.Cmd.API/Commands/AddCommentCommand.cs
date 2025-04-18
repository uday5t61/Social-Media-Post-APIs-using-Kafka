using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class AddCommentCommand : BaseCommand
    {
        public required string Comment { get; set; }
        public required string Username { get; set; }
    }
}