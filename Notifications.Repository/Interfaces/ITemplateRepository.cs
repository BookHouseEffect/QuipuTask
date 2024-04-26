using Notifications.Repository.Entities;

namespace Notifications.Repository.Interfaces
{
    public interface ITemplateRepository : IBaseRepository<Template>
    {
        Task<Template?> GetTemplateByIdAndClient(Guid templateId, Guid clientId);
    }
}
