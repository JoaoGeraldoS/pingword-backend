using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.StudyState;
using pingword.src.Models.StudyState;

namespace pingword.src.Repositories.StudyStates
{
    public class StudyStateRepository : IStudyStateRepository
    {
        private readonly AppDbContext _context;

        public StudyStateRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Study?> GetStudyByUserId(string UserId) =>
            await _context.Studies.FirstOrDefaultAsync(s => s.UserId == UserId);
       

        public async Task<List<Study>> GetStateActiveAsync(DateTime limitDate) =>
            await _context.Studies
                .Where(s => s.Status == Status.ACTIVE || s.Status == Status.RETURNING && s.LastInteraction < limitDate)
                .ToListAsync();

        public async Task AddStateAsync(Study study)
        {
            await _context.Studies.AddAsync(study);
            await _context.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
