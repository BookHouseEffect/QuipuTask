using Notifications.Core.Models;
using System.Net.Mail;

namespace Notifications.Core.Interfaces
{
    public interface IQueueSender
    {
        Task SendNotificationData(NotificationModel model);

        void SendMessage(MailMessage message);
    }
}
