using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Infra.Configuration;

namespace Rzx.Crm.Infra.Database
{
    public class SqliteDbContext : DbContextBase
    {
        public SqliteDbContext(DatabaseConfig databaseConfig, ILoggerFactory loggerFactory) : base(databaseConfig, loggerFactory) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"data source={databaseConfig.Host}")
                .ConfigureWarnings(p => p.Ignore(RelationalEventId.AmbientTransactionWarning));
        }
    }
}
