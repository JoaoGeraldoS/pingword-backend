using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Words;
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
        public async Task<ActionResult<List<WordResponseDto>>> GetWords()
        {
            Log.Information("Getting words for user");

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null) return Unauthorized();

            var words = await _wordService.GetWordsAsync(userClaim.Value);
            return Ok(words);
        }

        [Authorize]
        [HttpPost("sync")]
        public async Task<IActionResult> SyncWords([FromBody] List<WordUpdateRequestDto> words)
        {
            foreach (var word in words)
            {
                Console.WriteLine($"Word: {word.Words}, Translation: {word.Translation}, Example: {word.Example}, IsDeleted: {word.IsDeleted}, UpdatedAt: {word.UpdatedAt}");
            }

            Log.Information("Syncing words for user");
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null) return Unauthorized();

            await _wordService.SyncWords(words, userClaim.Value);
            return Ok();
        }


        [HttpPatch("{wordId}/intercation")]
        public async Task<IActionResult> InteractionWord(Guid wordId, [FromBody] WordIteractionDto interaction)
        {

            var userID = "29844e1e-804c-4846-9bad-8788a84ff354";
            await _wordService.WordInteractionUpdate(userID, wordId, interaction.wordInteraction);
            return Ok();
        }
        
    }
}
