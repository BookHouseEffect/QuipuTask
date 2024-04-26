using Notifications.Core.Interfaces;
using Notifications.Repository.Entities;
using Notifications.Repository.Interfaces;

namespace Notifications.Domain.Services
{
    public class ClientService(
        IClientRepository clientRepository
        ) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task RegisterClient(string clientName)
        {
            await _clientRepository.InsertAsync(new Client
            {
                Id = Guid.NewGuid(),
                Name = clientName,
            });
            await _clientRepository.CommitAsync();
        }

        public async Task<Client?> GetClient(string clientName)
        {
            return await _clientRepository.GetClientByName(clientName);
        }
    }
}
