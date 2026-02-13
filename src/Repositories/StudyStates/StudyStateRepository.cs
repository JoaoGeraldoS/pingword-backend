using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
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
        public async Task<Study?> GetStudyByUserId(string UserId)
        {
            return await _context.Studies.FirstOrDefaultAsync(s => s.UserId == UserId);
            
        }
    }
}
