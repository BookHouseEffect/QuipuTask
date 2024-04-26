using Microsoft.EntityFrameworkCore;
using Notifications.Repository.Entities;
using Notifications.Repository.Interfaces;

namespace Notifications.Repository.Repositories
{
    public class ClientRepository(AppDbContext context)
        : BaseRepository<Client>(context), IClientRepository
    {
        public async Task<Client?> GetClientByName(string clientName)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Name.ToLower() == clientName.ToLower());
        }
    }
}
