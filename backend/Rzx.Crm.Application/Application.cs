using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rzx.Crm.Api;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Services;
using Rzx.Crm.Infra.Configuration;
using Rzx.Crm.Infra.Database;
using Serilog;

namespace Rzx.Crm.Application
{
    public class Application
    {
        public static void Bootstrap(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .AddJsonFile("serilog.json", false)
                .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            var appConfig = new ApplicationConfig();
            configuration.Bind("Crm", appConfig);

            // configuration
            services.AddSingleton(appConfig.SeedData);
            services.AddSingleton(appConfig.Database);

            // business logic
            services.AddSingleton<EmployeeService>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<OrderService>();
            services.AddSingleton<SeedingService>();

            // infrastruction services
            services.AddMediatR(o => { o.RegisterServicesFromAssemblyContaining<CustomerService>(); o.RegisterServicesFromAssemblyContaining<WebHost>(); });
            services.AddSingleton<IDataRepository>(s =>
            {
                var baseRepo = new SqliteDataRepository(s.GetRequiredService<DatabaseConfig>(), s.GetRequiredService<ILoggerFactory>());
                return baseRepo;
            });

        }
    }
}

