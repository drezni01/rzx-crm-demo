using Microsoft.Extensions.Logging;
using Rzx.Crm.Infra.Configuration;

namespace Rzx.Crm.Infra.Database
{
    public class SqliteDataRepository : DataRepositoryBase
    {
        private readonly ILoggerFactory _loggerFactory;

        public SqliteDataRepository(DatabaseConfig databaseConfig, ILoggerFactory loggerFactory) :base(databaseConfig)
        {
            using var dbx = GetCtx();
            if (databaseConfig.IsTransient)
                dbx.Database.EnsureDeleted();
            dbx.Database.EnsureCreated();

            _loggerFactory = loggerFactory;
        }

        protected override DbContextBase GetCtx()
        {
            return new SqliteDbContext(databaseConfig, _loggerFactory);
        }
    }
}
