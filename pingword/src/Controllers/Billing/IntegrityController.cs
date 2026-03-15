using Microsoft.AspNetCore.Mvc;
using pingword.src.Services.Billing;

namespace pingword.src.Controllers.Billing
{
    [ApiController]
    [Route("api/integrity")]
    public class IntegrityController : ControllerBase
    {
        private readonly IntegrityService _integrityService;

        public IntegrityController(IntegrityService integrityService)
        {
            _integrityService = integrityService;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] IntegrityRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest(new { success = false, message = "Token vazio" });

            if (_environment.IsDevelopment())
            return Ok(new { success = true, debug = "Modo desenvolvimento" });


            try
            {
                var status = await _integrityService.VerifyTokenAsync(request.Token);

                Console.WriteLine($"[DEBUG] Play Integrity Status: {status}");

                if (status == "App Original e Seguro")
                    return Ok(new { success = true });

          
                return StatusCode(403, new { success = false, message = status });
            }
            catch (Exception ex)
            {
          
                return StatusCode(500, new { success = false, message = ex.Message, detail = ex.InnerException?.Message });
            }
        }
    }

    public class IntegrityRequest { public string Token { get; set; } }
}
