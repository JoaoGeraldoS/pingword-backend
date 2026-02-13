using pingword.src.DTOs.Notifications;
using pingword.src.Models.Notifications;

namespace pingword.src.Mappers.Notifications
{
    public static class NotificationMapper
    {
        public static NotificationResponseDto ToDto(this Notification notification)
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                Word = notification.Word,
                Language = notification.Language,
                CreatedAt = notification.CreatedAt,
                NotificationEnum = notification.NotificationEnum,
                Action = notification.Action
            };
        }
        public static Notification ToEntity(this NotificationRequestDto dto, string lang)
        {
            return new Notification
            {
                Word = dto.Word,
                Language = lang
            };
        }
    }
}
