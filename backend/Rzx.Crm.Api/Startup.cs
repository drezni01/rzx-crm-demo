namespace Rzx.Crm.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            WebApi.WebApi.BootstrapServices(services);
            SignalR.SignalR.BootstrapServices(services);

            services.AddHostedService<AppStartupService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WebApi.WebApi.ConfigureApp(app, env);
            SignalR.SignalR.ConfigureApp(app, env);
        }
    }
}
