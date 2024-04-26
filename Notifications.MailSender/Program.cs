using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Notifications.Core.Configuration;
using Notifications.Core.Interfaces;
using Notifications.Domain.MessageQueue;
using System.Net.Mail;


namespace Notifications.MailSender
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

            MailConfiguration? mailConfig = configuration.GetSection("Mail").Get<MailConfiguration>() ??
                throw new Exception("Mail environment variables not defined.");
            IOptions<MailConfiguration> mailConfigOptions = Options.Create<MailConfiguration>(mailConfig);

            IMessageQueueProcessor messageQueueProcessor = new MessageQueueProcessor(rabbitMqConfigOptions, mailConfigOptions);

            while (true)
            {
                try
                {
                    MailMessage? message = messageQueueProcessor.GetMessageFromQueue();
                    if (message == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    messageQueueProcessor.SendMailAsync(message).RunSynchronously();
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
