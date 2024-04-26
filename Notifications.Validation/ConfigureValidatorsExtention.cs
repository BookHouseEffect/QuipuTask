using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Core.Models;
using Notifications.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Validation
{
    public static class ConfigureValidatorsExtention
    {
        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<NotificationModel>, NotificationModelValidator>();

            return services;
        }
    }
}
