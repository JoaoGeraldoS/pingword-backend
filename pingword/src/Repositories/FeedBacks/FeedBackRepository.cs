using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Interfaces.FeedBacks;
using pingword.src.Models.FeedBacks;

namespace pingword.src.Repositories.FeedBacks
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly AppDbContext _context;

        public FeedBackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddFeedBack(FeedBack feedBack)
        {
            await _context.FeedBacks.AddAsync(feedBack);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeedBack>> GetAllFeedBacks() => 
            await _context.FeedBacks
                .AsNoTracking().ToListAsync();
        

        public async Task<FeedBack?> GetFeedBackById(Guid id) => 
            await _context.FeedBacks
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
       

        public async Task RemoveFeedBack(FeedBack feedBack)
        {
            _context.FeedBacks.Remove(feedBack);
            await _context.SaveChangesAsync();
        }
    }
}
