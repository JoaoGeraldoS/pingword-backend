using pingword.DTOs.Notifications;
using pingword.Enums.Notifications;

namespace pingword.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> AddNotificationAsync(string userId, NotificationRequestDto request);
        Task<NotificationResponseDto> UpdateNotificationAsync(Guid id, NotificationEnum status);
    }
}
