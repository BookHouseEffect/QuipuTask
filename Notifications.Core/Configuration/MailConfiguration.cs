namespace Notifications.Core.Configuration
{
    public class MailConfiguration
    {
        public required string Host {  get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool EnableSsl { get; set; }
        public required string From { get; set; }
    }
}
