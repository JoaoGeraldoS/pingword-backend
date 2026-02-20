using Microsoft.AspNetCore.Mvc;

namespace pingword.src.Controllers.Ping
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok("pong");
        }
    }
}
