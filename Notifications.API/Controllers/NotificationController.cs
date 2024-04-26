using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Notifications.Core.Interfaces;
using Notifications.Core.Models;

namespace Notifications.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NotificationController(
        ILogger<NotificationController> logger,
        IValidator<NotificationModel> notificationValidator,
        IQueueSender queueSender
    ) : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger = logger;
        private readonly IValidator<NotificationModel> _notificationValidator = notificationValidator;
        private readonly IQueueSender _queueSender = queueSender;

        /// <summary>
        /// Validates the notification request, and if valid, adds it to the message queue, otherwise it returns error
        /// </summary>
        /// <param name="model">The model for the notifications</param>
        /// <returns>Successfully added to message queue or an error</returns>
        /// <response code="204">Successfully added to message queue</response>
        /// <response code="400">Validation error occurred</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostNotification([FromBody] NotificationModel model)
        {
            ValidationResult result = await _notificationValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                string errorString = result.ToString();
                _logger.LogError("Notification model not valid: {errorString}", errorString);
                return this.BadRequest(result);
            }

            await _queueSender.SendNotificationData(model);

            return this.NoContent();
        }
    }
}
