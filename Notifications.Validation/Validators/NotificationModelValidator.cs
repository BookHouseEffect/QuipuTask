using FluentValidation;
using Notifications.Core.Models;

namespace Notifications.Validation.Validators
{
    public class NotificationModelValidator : AbstractValidator<NotificationModel>
    {
        public NotificationModelValidator()
        {
            this.RuleFor(x => x.ClientId).NotEmpty();

            this.RuleFor(x => x.TemplateId).NotEmpty();

            this.RuleFor(x => x.Recipients)
                .NotEmpty()
                .ForEach(x => x.NotEmpty().EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible));               
        }
    }
}
