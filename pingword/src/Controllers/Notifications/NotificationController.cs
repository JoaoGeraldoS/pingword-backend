using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Notifications;
using pingword.src.Interfaces.Notifications;
using System.Security.Claims;

namespace pingword.src.Controllers.Notifications
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService service, ILogger<NotificationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<NotificationResponseDto>> AddNotification([FromBody] NotificationRequestDto request)
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            _logger.LogInformation("Adding notification for user {UserId} with word {Word}", userClaim.Value, request.Word);



            var result = await _service.AddNotificationAsync(userClaim.Value, request);
            return Ok(result);
        }

        [HttpPatch("{notificationId}/action")]
        public async Task<ActionResult> UpdateNotification(Guid notificationId, [FromBody] UpdateNotificationDto status)
        {
            _logger.LogInformation("Updating notification {NotificationId} with status {Status}", notificationId, status);
            var result = await _service.UpdateNotificationAsync(notificationId, status.Notification);
            return Ok(result);
        }
    }
}
