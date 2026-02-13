using Microsoft.EntityFrameworkCore;
using pingword.Data;
using pingword.Interfaces.Notifications;
using pingword.Models.Notifications;

namespace pingword.Repositories
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
