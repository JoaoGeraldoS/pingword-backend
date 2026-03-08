using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.FeedBacks;
using pingword.src.Interfaces.FeedBacks;
using Serilog;

namespace pingword.src.Controllers.FeedBacks
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        private readonly ILogger<FeedBackController> _logger;
        public FeedBackController(IFeedBackService feedBackService, ILogger<FeedBackController> logger)
        {
            _feedBackService = feedBackService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFeedBack([FromBody] FeedBackRequestDto request)
        {
            _logger.LogInformation("Creating feedback from user {User}", request.User);
            await _feedBackService.AddFeedBackAsync(request);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedBackResponseDto>>> GetFeedBacks()
        {
            _logger.LogInformation("Retrieving all feedbacks");
            var feedBacks = await _feedBackService.GetAllFeedBacks();
            return Ok(feedBacks);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedBack(Guid id)
        {
            
            _logger.LogInformation("Deleting feedback with id {FeedBackId}", id);

            await _feedBackService.RemoveFeedBackAsync(id);
            return NoContent();
            
        }
    }
}
