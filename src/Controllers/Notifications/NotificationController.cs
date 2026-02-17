using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Notifications;
using pingword.src.Enums.Notifications;
using pingword.src.Interfaces.Notifications;

namespace pingword.src.Controllers.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService service, ILogger<NotificationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> AddNotification(string userId, [FromBody] NotificationRequestDto request)
        {
            var result = await _service.AddNotificationAsync(userId, request);
            return Ok(result);
        }

        [HttpPatch("{notificationId}/action")]
        public async Task<ActionResult<NotificationResponseDto>> UpdateNotification(Guid notificationId, [FromBody] UpdateNotificationDto status)
        {
            var result = _service.UpdateNotificationAsync(notificationId, status.Notification);
            return Ok(result);
        }
    }
}
