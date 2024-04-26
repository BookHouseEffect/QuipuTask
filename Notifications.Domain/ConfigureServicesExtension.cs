using Microsoft.Extensions.DependencyInjection;
using Notifications.Core.Interfaces;
using Notifications.Domain.MessageQueue;
using Notifications.Domain.Services;

namespace Notifications.Domain
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IQueueSender, QueueSender>();
            services.AddScoped<INotificationQueueProcessor, NotificationQueueProcessor>();
            services.AddScoped<IMailRenderer, MailRenderer>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<XmlReaderService>();

            return services;
        }
    }
}
