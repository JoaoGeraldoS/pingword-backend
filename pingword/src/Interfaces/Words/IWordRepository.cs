using pingword.src.Enums.Users;
using pingword.src.Models.Words;

namespace pingword.src.Interfaces.Words
{
    public interface IWordRepository
    {
        Task AddWord(Word word);
        Task<List<Word>> GetAllWords(string userId, UserLevelEnum userLevel);
        Task<Word?> GetByIdInternal(Guid id);
        Task<Word?> GetByIdWithUser(string userId,Guid id);
        Task<int> CountAsync(string userId);

        void UpdateWrod(Word word);

        void DeleteWord(Word word);
        Task SaveChangesAsync();
        Task<Word?> GetByText(string userId, string text, string translation);
        void DeleteWordAdmin(Word word);


    }
}
