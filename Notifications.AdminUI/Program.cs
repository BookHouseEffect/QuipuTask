using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.Core.Configuration;
using Notifications.Domain;
using Notifications.Repository;

namespace Notifications.AdminUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<MainUI>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureServices((context, services) => {
                    services.AddOptions();
                    services.Configure<RabbitMqConfiguration>(context.Configuration.GetSection("RabbitMQ"));
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        var connOptions = context.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfiguration>();
                        options.UseSqlServer(connOptions?.SQLServer);
                    }, ServiceLifetime.Transient);

                    services.ConfigureDomainServices();
                    services.ConfigureRepositoryServices();
                    services.AddTransient<MainUI>();
                });
        }
    }
}