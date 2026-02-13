using pingword.src.Models.Notifications;

namespace pingword.src.Interfaces.Notifications
{
    public interface INotificationRepository
    {
        Task<Notification?> GetNotificationById(Guid id);
    }
}
