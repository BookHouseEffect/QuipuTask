using Microsoft.Extensions.DependencyInjection;
using Notifications.Repository.Interfaces;
using Notifications.Repository.Repositories;

namespace Notifications.Repository
{
    public static class ConfigureRepositoryExtension
    {
        public static IServiceCollection ConfigureRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();

            return services;
        }
    }
}
