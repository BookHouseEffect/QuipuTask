using Notifications.Core.Models;

namespace Notifications.Core.Interfaces
{
    public interface INotificationQueueProcessor
    {
        AdvancedNotificationModel? ReceiveNotification();
    }
}
