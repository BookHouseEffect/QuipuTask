namespace Notifications.Repository.Entities
{
    public class Template : BaseEntity
    {
        public required string Name { get; set; }
        public required string Sender { get; set; }
        public required string Subject { get; set; }
        public required string Content { get; set; }
        public bool IsContentHtml { get; set; }
        public required Guid OwnerId { get; set; }

        public required virtual Client Owner { get; set; }
    }
}
