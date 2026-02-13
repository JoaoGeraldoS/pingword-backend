using Microsoft.AspNetCore.Mvc;
using pingword.DTOs.Notifications;
using pingword.Interfaces.Notification;

namespace pingword.Controllers.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> AddNotification(string userId, [FromBody] NotificationRequestDto request)
        {
            var result = await _service.AddNotificationAsync(userId, request);
            return Ok(result);
        }
    }
}
