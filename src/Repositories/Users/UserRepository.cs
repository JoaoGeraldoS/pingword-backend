using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.DTOs.Users;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;
using pingword.src.Models.Users;

namespace pingword.src.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            
        }

        public async Task<Study?> GetStudyByUserId(string UserId) =>
            await _context.Studies
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == UserId);

        public IQueryable<Notification> GetUserNotificationsQuery(string userId)
        {
            return _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.Action != null);
        }
    }
}
