using pingword.Enums.Notifications;

namespace pingword.DTOs.Notifications
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
