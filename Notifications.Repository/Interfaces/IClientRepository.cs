using Notifications.Repository.Entities;

namespace Notifications.Repository.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<Client?> GetClientByName(string clientName);
    }
}
