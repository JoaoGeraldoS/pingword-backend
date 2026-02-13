using Microsoft.EntityFrameworkCore;
using pingword.Data;
using pingword.Interfaces.StudyState;
using pingword.Models.StudyState;

namespace pingword.Repositories.StudyStates
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
