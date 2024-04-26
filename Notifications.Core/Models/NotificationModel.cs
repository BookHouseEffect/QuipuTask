namespace Notifications.Core.Models
{
    public class NotificationModel
    {
        public required Guid ClientId { get; set; }
        public required Guid TemplateId { get; set; }
        public required IList<string> Recipients { get; set; }
        public Dictionary<string, string>? TemplateData { get; set; }
    }
}
