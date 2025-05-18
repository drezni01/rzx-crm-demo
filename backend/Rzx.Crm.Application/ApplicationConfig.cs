using Rzx.Crm.Core.Configuration;
using Rzx.Crm.Infra.Configuration;

namespace Rzx.Crm.Application
{
    public class ApplicationConfig
    {
        public SeedDataConfig SeedData { get; set; }
        public DatabaseConfig Database { get; set; }
    }
}
