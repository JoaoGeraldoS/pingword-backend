using pingword.src.Models.Words;

namespace pingword.src.Interfaces.Words
{
    public interface IWordRepository
    {
        Task AddWord(Word word);
        Task<List<Word>> GetAllWords(string userId);
        Task<Word?> GetWordByUser(string userId);
        Task<Word?> GetById(string userId, Guid id);
        Task DeleteWord(Word word);
        Task UpdateWordInteractio(Word word);
        Task SaveChangesAsync();
        
    }
}
