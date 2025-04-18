using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;

namespace Post.Cmd.API.Commands
{
    public class NewPostCommand :BaseCommand
    {
        public required string Author { get; set; }
        public required string Message { get; set; }
    }
}