using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
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

        public async Task DeleteWord(Word word)
        {
            _context.Update(word);
        }

        public async Task<List<Word>> GetAllWords(string userId)
        {
            return await _context.Words
                .AsTracking()
                .Where(w => w.UserId == userId && w.IsDeletd == false)
                .ToListAsync();
        }

        public async Task<Word?> GetById(string userId, Guid id)
        {
            return await _context.Words
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        }

        public Task<Word?> GetWordByUser(string userId)
        {
            return _context.Words
                .AsNoTracking()
                .Include(w => w.Users)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWordInteractio(Word word)
        {
            _context.Words.Update(word);
            
        }
    }
}
