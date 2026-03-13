using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Enums.Users;
using pingword.src.Enums.Words;
using pingword.src.Interfaces.Words;
using pingword.src.Models.Words;

namespace pingword.src.Repositories.Words
{
    public class WordRepository : IWordRepository
    {
        private readonly AppDbContext _context;

        public WordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddWord(Word word)
        {
            await _context.Words.AddAsync(word);
        }

        public void DeleteWord(Word word)
        {
            _context.Update(word);
        }

        public async Task<List<Word>> GetAllWords(string userId, UserLevelEnum userLevel)
        {
            return await _context.Words
                .AsTracking()
                .Where(w =>
                    !w.IsDeleted &&
                        (
                            w.UserId == userId ||
                            (w.WordEnum == WordEnum.SYSTEM && w.UserLevel == userLevel)
                        )
                    )
                .ToListAsync();
        }

        public async Task<Word?> GetByIdInternal(Guid id)
        {
            return await _context.Words
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Word?> GetByIdWithUser(string userId, Guid id)
        {
            return await _context.Words
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        }

        public async Task<int> CountAsync(string userId)
        {
            return await _context.Words.CountAsync(w => w.UserId == userId);
        }

        public async Task<Word?> GetByText(string userId, string text, string translation)
        {
            return await _context.Words
                .FirstOrDefaultAsync(w => w.UserId == userId
                        && w.Words.ToLower() == text.ToLower()
                        && w.Translation.ToLower() == translation.ToLower()
                        && !w.IsDeleted);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateWrod(Word word)
        {
            _context.Words.Update(word);
        }

        public void DeleteWordAdmin(Word word)
        {
             _context.Words.Remove(word);
        }
    }
}
