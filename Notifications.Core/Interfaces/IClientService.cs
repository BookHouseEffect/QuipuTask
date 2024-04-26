using Notifications.Repository.Entities;

namespace Notifications.Core.Interfaces
{
    public interface IClientService
    {
        Task RegisterClient(string clientName);

        Task<Client?> GetClient(string clientName);
    }
}
