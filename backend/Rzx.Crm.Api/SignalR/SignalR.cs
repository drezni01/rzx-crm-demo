using Newtonsoft.Json;

namespace Rzx.Crm.Api.SignalR
{
    public class SignalR
    {
        public static void BootstrapServices(IServiceCollection services)
        {
            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.PayloadSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            services.AddSingleton<MessageHub>();
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/ws");
            });
        }
    }
}
