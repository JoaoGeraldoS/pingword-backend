using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.FeedBacks;
using pingword.src.Interfaces.FeedBacks;

namespace pingword.src.Controllers.FeedBacks
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeedBack([FromBody] FeedBackRequestDto request)
        {
            await _feedBackService.AddFeedBackAsync(request);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedBackResponseDto>>> GetFeedBacks()
        {
            var feedBacks = await _feedBackService.GetAllFeedBacks();
            return Ok(feedBacks);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeedBack([FromQuery] Guid id)
        {
            await _feedBackService.RemoveFeedBackAsync(id);
            return Ok();
        }
    }
}
