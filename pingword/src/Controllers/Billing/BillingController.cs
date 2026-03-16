using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Billing;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using pingword.src.Services.Billing;
using System.Security.Claims;

namespace pingword.src.Controllers.Billing
{
    [ApiController]
    [Route("api/billing")]
    public class BillingController : ControllerBase
    {
        private readonly GooglePlayService _googlePlayService;
        private readonly UserManager<User> _userManager;
        

        public BillingController(GooglePlayService googlePlayService, UserManager<User> userManager)
        {
            _googlePlayService = googlePlayService;
            _userManager = userManager;
            
        }

        [Authorize]
        [HttpPost("validate-premium")]
        public async Task<IActionResult> ValidatePremium([FromBody] ValidatePremiumRequest request)
        {
            if (string.IsNullOrEmpty(request.PurchaseToken))
                return BadRequest("PurchaseToken é obrigatório");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _googlePlayService.ValidatePremium(request.PurchaseToken, userId);

            if (!result)
                return BadRequest("Falha ao validar assinatura");

            var newToken = await _googlePlayService.NewTokenJWT(userId);

            return Ok(new
            {
                message = "Premium ativado com sucesso",
                accessToken = newToken, 
                isPremium = true
            });
        }

        [HttpPost("google-play-webhook")]
        public IActionResult GoogleWebhook([FromBody] object body)
        {
            Console.WriteLine(body);
            return Ok();
        }

        [Authorize]
        [HttpGet("restore")]
        public async Task<IActionResult> Restore()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _googlePlayService.Restore(userId!);

            if (user == null)
                return NotFound();

            if (user.PremiumUntil != null && user.PremiumUntil > DateTime.UtcNow)
            {
                return Ok(new
                {
                    isPremium = true,
                    premiumUntil = user.PremiumUntil
                });
            }

            return Ok(new
            {
                isPremium = false
            });
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _googlePlayService.Restore(userId!);

            bool isPremium = user.PremiumUntil != null &&
                             user.PremiumUntil > DateTime.UtcNow;

            return Ok(new
            {
                isPremium = isPremium,
                premiumUntil = user.PremiumUntil
            });
        }
    }
}
