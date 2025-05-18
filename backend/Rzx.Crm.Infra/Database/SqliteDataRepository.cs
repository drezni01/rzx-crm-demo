using Microsoft.Extensions.Logging;
using Rzx.Crm.Infra.Configuration;

namespace Rzx.Crm.Infra.Database
{
    public class SqliteDataRepository : DataRepositoryBase
    {
        private ILoggerFactory loggerFactory;

        public SqliteDataRepository(DatabaseConfig databaseConfig, ILoggerFactory loggerFactory) :base(databaseConfig)
        {
            using var dbx = GetCtx();
            if (databaseConfig.IsTransient)
                dbx.Database.EnsureDeleted();
            dbx.Database.EnsureCreated();

            this.loggerFactory = loggerFactory;
        }

        protected override DbContextBase GetCtx()
        {
            return new SqliteDbContext(databaseConfig, loggerFactory);
        }
    }
}
