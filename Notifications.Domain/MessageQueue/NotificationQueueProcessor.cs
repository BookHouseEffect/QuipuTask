using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notifications.Core.Configuration;
using Notifications.Core.Interfaces;
using Notifications.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Notifications.Domain.MessageQueue
{
    public class NotificationQueueProcessor(
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration
        ) : BaseQueue(rabbitMqConfiguration), INotificationQueueProcessor
    {
        public AdvancedNotificationModel? ReceiveNotification()
        {
            using IConnection connection = GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(_rabbitMqConfiguration.NotificationQueueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            BasicGetResult result = channel.BasicGet(_rabbitMqConfiguration.NotificationQueueName, true);
            if (result == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<AdvancedNotificationModel>(
                Encoding.UTF8.GetString(result.Body.ToArray()));
        }
    }
}
