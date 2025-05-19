
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rzx.Crm.Application;
using Rzx.Crm.Core.Interfaces;
using Rzx.Crm.Core.Services;
using Rzx.Crm.Infra.Configuration;
using Rzx.Crm.Infra.Database;
using Serilog;

namespace Rzx.Crm.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        private static IServiceProvider _serviceProvider;

        public TestBase()
        {
            lock (typeof(TestBase))
            {
                if (_serviceProvider == null)
                    _serviceProvider = CreateServiceProvider();
            }
        }

        protected IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        private ApplicationConfig GetAppConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("serilog.json", false)
                .Build();

            var appConfig = new ApplicationConfig();
            config.Bind("Crm", appConfig);
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            return appConfig;
        }

        private IServiceProvider CreateServiceProvider()
        {
            var appConfig = GetAppConfig();
            var services = new ServiceCollection();

            services.AddSingleton(appConfig.SeedData);
            services.AddSingleton(appConfig.Database);

            services.AddSingleton<EmployeeService>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<OrderService>();
            services.AddSingleton<SeedingService>();

            services.AddMediatR(o => { o.RegisterServicesFromAssemblyContaining<CustomerService>(); });
            services.AddSingleton<IDataRepository>(s =>
            {
                var baseRepo = new SqliteDataRepository(s.GetRequiredService<DatabaseConfig>(), s.GetRequiredService<ILoggerFactory>());
                return baseRepo;
            });
            services.AddLogging(logging => logging.AddSerilog());

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        [TestInitialize]
        public async Task ClearAllAsync()
        {
            var orderSvc = ServiceProvider.GetRequiredService<OrderService>();
            await orderSvc.DeleteAllOrdersAsync();

            var productSvc = ServiceProvider.GetRequiredService<ProductService>();
            await productSvc.DeleteAllProductsAsync();

            var customerSvc = ServiceProvider.GetRequiredService<CustomerService>();
            await customerSvc.DeleteAllCustomersAsync();

            var employeeSvc = ServiceProvider.GetRequiredService<EmployeeService>();
            await employeeSvc.DeleteAllEmployeesAsync();
        }
    }
}
