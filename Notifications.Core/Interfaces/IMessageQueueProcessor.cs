using System.Net.Mail;

namespace Notifications.Core.Interfaces
{
    public interface IMessageQueueProcessor
    {
        MailMessage? GetMessageFromQueue();

        Task SendMailAsync(MailMessage message);
    }
}
