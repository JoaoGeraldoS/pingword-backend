using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace pingword.src.Controllers.Ping
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Health()
        {
            Log.Information("Health check endpoint called");
            return Ok("pong");
        }
    }
}
