using pingword.Models.Notifications;

namespace pingword.Interfaces.Notifications
{
    public interface INotificationRepository
    {
        Task<Notification?> GetNotificationById(Guid id);
    }
}
