using Microsoft.EntityFrameworkCore;
using Notifications.Repository.Entities;

namespace Notifications.Repository
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Template> Templates { get; set; }
    }
}
