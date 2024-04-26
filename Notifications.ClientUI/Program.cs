using Microsoft.EntityFrameworkCore;
using Notifications.Core.Configuration;
using Notifications.Domain;
using Notifications.Repository;

namespace Notifications.ClientUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddOptions();
            builder.Services.Configure<ConnectionStringConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddSingleton(builder.Configuration);

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connOptions = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfiguration>();
                options.UseSqlServer(connOptions?.SQLServer);
            }, ServiceLifetime.Scoped);

            builder.Services.ConfigureDomainServices();
            builder.Services.ConfigureRepositoryServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
