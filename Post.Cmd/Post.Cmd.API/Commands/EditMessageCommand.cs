using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class EditMessageCommand : BaseCommand
    {
        public required string Message { get; set; }
    }
}