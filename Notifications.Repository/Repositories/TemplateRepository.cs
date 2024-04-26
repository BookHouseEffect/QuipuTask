using Microsoft.EntityFrameworkCore;
using Notifications.Repository.Entities;
using Notifications.Repository.Interfaces;

namespace Notifications.Repository.Repositories
{
    public class TemplateRepository(AppDbContext context) 
        : BaseRepository<Template>(context), ITemplateRepository
    {
        public async Task<Template?> GetTemplateByIdAndClient(Guid templateId, Guid clientId)
        {
            return await _dbSet
                .Where(x => x.Id == templateId && x.OwnerId == clientId)
                .SingleOrDefaultAsync();
        }
    }
}
