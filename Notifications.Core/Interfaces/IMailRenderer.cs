using Notifications.Core.Models;
using System.Net.Mail;

namespace Notifications.Core.Interfaces
{
    public interface IMailRenderer
    {
        MailMessage RenderMail(AdvancedNotificationModel model);
    }
}
