using pingword.src.DTOs.Words;
using pingword.src.Enums.Users;
using pingword.src.Enums.Words;

namespace pingword.src.Interfaces.Words
{
    public interface IWordService
    {
        Task<List<WordUpdateRequestDto>> GetWordsAsync(string userId, UserLevelEnum userLevel);
        Task SyncWords(List<WordUpdateRequestDto> words, string userId);
        Task WordInteractionUpdate(string userId, Guid id, WordInteractionEnum iteraction);
        Task DeleteWordAdmin(Guid wordId);
    }
}
