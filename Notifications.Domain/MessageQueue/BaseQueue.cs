using Microsoft.Extensions.Options;
using Notifications.Core.Configuration;
using RabbitMQ.Client;

namespace Notifications.Domain.MessageQueue
{
    public abstract class BaseQueue(
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration
        )
    {
        protected readonly RabbitMqConfiguration _rabbitMqConfiguration = rabbitMqConfiguration.Value;

        protected IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new()
            {
                HostName = _rabbitMqConfiguration.HostName,
                UserName = _rabbitMqConfiguration.UserName,
                Password = _rabbitMqConfiguration.Password
            };
            return connectionFactory.CreateConnection();
        }
    }
}
