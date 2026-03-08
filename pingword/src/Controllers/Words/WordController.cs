using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Words;
using pingword.src.Enums.Users;
using pingword.src.Interfaces.Words;
using Serilog;
using System.Security.Claims;

namespace pingword.src.Controllers.Words
{
    [ApiController]
    [Route("api/words")]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;

        public WordController(IWordService service)
        {
            _wordService = service;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<WordUpdateRequestDto>>> GetWords([FromQuery] UserLevelEnum level)
        {
            Log.Information("Getting words for user");

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null) return Unauthorized();

            var words = await _wordService.GetWordsAsync(userClaim.Value, level);
            return Ok(words);
        }

        [Authorize]
        [HttpPost("sync")]
        public async Task<IActionResult> SyncWords([FromBody] List<WordUpdateRequestDto> words)
        {
            Log.Information("Syncing words for user");
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            await _wordService.SyncWords(words, userClaim.Value);
            return Ok();
        }


        [Authorize]
        [HttpPatch("{wordId}/interaction")]
        public async Task<IActionResult> InteractionWord(Guid wordId, [FromBody] WordIteractionDto interaction)
        {

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

           Log.Information("Updating word interaction for user {UserId} and word {WordId} with interaction {Interaction}", userClaim.Value, wordId, interaction.wordInteraction);

            await _wordService.WordInteractionUpdate(userClaim.Value, wordId, interaction.wordInteraction);
            return Ok();
        }
        
    }
}
