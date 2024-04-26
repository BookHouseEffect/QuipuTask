using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Notifications.Core.Configuration;
using Notifications.Domain;
using Notifications.Repository;
using Notifications.Validation;
using System.Reflection;

namespace Notifications.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GjorcheMedia",
                    Description = "API for posting notifications"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddOptions();
            builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
            builder.Services.Configure<ConnectionStringConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddSingleton(builder.Configuration);

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connOptions = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfiguration>();
                options.UseSqlServer(connOptions?.SQLServer);
            }, ServiceLifetime.Transient);

            builder.Services.ConfigureValidators();
            builder.Services.ConfigureDomainServices();
            builder.Services.ConfigureRepositoryServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
