using pingword.src.Enums.Notifications;

namespace pingword.src.DTOs.Notifications
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotificationEnum? NotificationEnum { get; set; }
        public DateTime? Action { get; set; }
    }
}
