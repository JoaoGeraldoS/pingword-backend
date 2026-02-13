using Microsoft.EntityFrameworkCore;
using pingword.src.Data;
using pingword.src.Interfaces.Notifications;
using pingword.src.Models.Notifications;

namespace pingword.src.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Notification?> GetNotificationById(Guid id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
          
        }
    }
}
