namespace Notifications.Core.Configuration
{
    public class RabbitMqConfiguration
    {
        public required string HostName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public required string NotificationQueueName { get; set; }
        public required string MailMessageQueueName { get; set; }
    }
}
