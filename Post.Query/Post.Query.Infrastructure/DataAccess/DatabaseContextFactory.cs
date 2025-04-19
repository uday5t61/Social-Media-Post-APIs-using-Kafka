using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbcontext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbcontext)
        {
            _configureDbcontext = configureDbcontext;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDbcontext(optionsBuilder);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
