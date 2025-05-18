
namespace Rzx.Crm.Infra.Configuration
{
    public class DatabaseConfig
    {
        public bool IsTransient { get; set; }
        public string Host { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
