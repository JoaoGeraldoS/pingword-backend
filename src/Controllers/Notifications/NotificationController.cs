using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Notifications;
using pingword.src.Interfaces.Notifications;

namespace pingword.src.Controllers.Notifications
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
