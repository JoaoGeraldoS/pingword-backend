using pingword.src.DTOs.Words;
using pingword.src.Enums.Words;

namespace pingword.src.Interfaces.Words
{
    public interface IWordService
    {
        Task<List<WordResponseDto>> GetWordsAsync(string userId);
        Task SyncWords(List<WordUpdateRequestDto> words, string userId);
        Task WordInteractionUpdate(string userId, Guid id, WordInteractionEnum iteraction);
    }
}
