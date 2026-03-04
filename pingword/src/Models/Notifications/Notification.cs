using pingword.src.Enums.Notifications;
using pingword.src.Models.Users;

namespace pingword.src.Models.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotificationEnum? NotificationEnum { get; set; }
        public DateTime? Action { get; set; }

        public User? User { get; set; }

    }
}
