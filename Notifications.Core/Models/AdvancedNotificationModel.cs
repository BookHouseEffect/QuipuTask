namespace Notifications.Core.Models
{
    public class AdvancedNotificationModel : NotificationModel
    {
        public required string Sender { get; set; }
        public required string Subject { get; set; }
        public required string Content { get; set; }

        public bool IsContentHtml { get; set; }
    }
}
