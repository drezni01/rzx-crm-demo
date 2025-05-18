using Rzx.Crm.Api;

namespace Rzx.Crm.Application
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WebHost.StartWebHost(args, Application.Bootstrap);
        }
    }
}
