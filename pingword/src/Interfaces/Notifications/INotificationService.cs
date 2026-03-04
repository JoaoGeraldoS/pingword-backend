using pingword.src.DTOs.Notifications;
using pingword.src.Enums.Notifications;

namespace pingword.src.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> AddNotificationAsync(string userId, NotificationRequestDto request);
        Task<NotificationResponseDto> UpdateNotificationAsync(Guid id, NotificationEnum status);
    }
}
