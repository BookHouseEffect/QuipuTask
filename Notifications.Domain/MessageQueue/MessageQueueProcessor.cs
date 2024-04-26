using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notifications.Core.Configuration;
using Notifications.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Notifications.Domain.MessageQueue
{
    public class MessageQueueProcessor(
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
        IOptions<MailConfiguration> mailConfiguration
        ) : BaseQueue(rabbitMqConfiguration), IMessageQueueProcessor
    {
        private readonly MailConfiguration _mailConfiguration = mailConfiguration.Value;

        public MailMessage? GetMessageFromQueue()
        {
            using IConnection connection = GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(_rabbitMqConfiguration.MailMessageQueueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            BasicGetResult result = channel.BasicGet(_rabbitMqConfiguration.MailMessageQueueName, true);
            if (result == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<MailMessage>(Encoding.UTF8.GetString(result.Body.ToArray()));
        }

        public async Task SendMailAsync(MailMessage message)
        {
            SmtpClient smtpClient = new(_mailConfiguration.Host)
            {
                Port = _mailConfiguration.Port,
                Credentials = new NetworkCredential
                {
                    UserName = _mailConfiguration.Username,
                    Password = _mailConfiguration.Password
                },
                EnableSsl = _mailConfiguration.EnableSsl
            };

            message.From = new MailAddress(_mailConfiguration.From);

            await smtpClient.SendMailAsync(message);
        }
    }
}
