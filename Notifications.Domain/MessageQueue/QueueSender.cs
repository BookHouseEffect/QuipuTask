using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notifications.Core.Configuration;
using Notifications.Core.Interfaces;
using Notifications.Core.Models;
using Notifications.Repository.Interfaces;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Text;

namespace Notifications.Domain.MessageQueue
{
    public class QueueSender(
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
        IClientRepository? clientRepository,
        ITemplateRepository? templateRepository
        ) : BaseQueue(rabbitMqConfiguration), IQueueSender 
    {
        private readonly IClientRepository? _clientRepository = clientRepository;
        private readonly ITemplateRepository? _templateRepository = templateRepository;

        public async Task SendNotificationData(NotificationModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(_clientRepository);
            ArgumentNullException.ThrowIfNull(_templateRepository);

            var client = _clientRepository.GetByID(model.ClientId);
            var template = await _templateRepository.GetByID(model.TemplateId);

            if (client == null || template == null)
            {
                throw new InvalidOperationException("Client and/or template not found");
            }

            var mqModel = new AdvancedNotificationModel()
            {
                ClientId = model.ClientId,
                TemplateId = model.TemplateId,
                Recipients = model.Recipients,
                TemplateData = model.TemplateData,
                Sender = template.Sender,
                Subject = template.Subject,
                Content = template.Content,
                IsContentHtml = template.IsContentHtml,
            };

            using IConnection connection = GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(_rabbitMqConfiguration.NotificationQueueName, true, false, false, null);
            channel.BasicPublish(string.Empty, _rabbitMqConfiguration.NotificationQueueName, null,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mqModel)));
        }

        public void SendMessage(MailMessage message)
        {
            using IConnection connection = GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(_rabbitMqConfiguration.MailMessageQueueName, true, false, false, null);
            channel.BasicPublish(string.Empty, _rabbitMqConfiguration.MailMessageQueueName, null,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }
    }
}
