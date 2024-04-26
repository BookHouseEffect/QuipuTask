using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Notifications.Core.Configuration;
using Notifications.Core.Interfaces;
using Notifications.Core.Models;
using Notifications.Domain.MessageQueue;
using Notifications.Domain.Services;
using System.Net.Mail;

namespace Notifications.Processor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration configuration = builder.Build();

            RabbitMqConfiguration? rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqConfiguration>() 
                ?? throw new Exception("RabbitMQ environment variables not defined.");
            IOptions<RabbitMqConfiguration> rabbitMqConfigOptions = Options.Create<RabbitMqConfiguration>(rabbitMqConfig);

            INotificationQueueProcessor notificationProcessor = new NotificationQueueProcessor(rabbitMqConfigOptions);
            IQueueSender queueSender = new QueueSender(rabbitMqConfigOptions, null, null);
            IMailRenderer mailRenderer = new MailRenderer();

            while (true)
            {
                try
                {
                    AdvancedNotificationModel? notification = notificationProcessor.ReceiveNotification();
                    if (notification == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ClientId: {0} TemplateId: {1}", notification.ClientId, notification.TemplateId);
                    Console.ForegroundColor = ConsoleColor.White;

                    MailMessage message = mailRenderer.RenderMail(notification);
                    queueSender.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
            }
        }
    }
}
