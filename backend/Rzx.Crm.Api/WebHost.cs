using Serilog;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Rzx.Crm.Api
{
    public class WebHost
    {
        public static void StartWebHost(string[] args, Action<IServiceCollection> diAction)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(diAction);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 8080, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        });
                    });
                })
                .UseSerilog()
                .Build();

            host.Run();
        }
    }
}
