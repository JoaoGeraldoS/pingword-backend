using Microsoft.AspNetCore.Identity;
using pingword.src.Enums.Users;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;
using pingword.src.Models.Words;

namespace pingword.src.Models.Users
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string? Language {  get; set; }
        public DateTime TimeZone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserLevelEnum UserLevel { get; set; }
        public bool IsPremium { get; set; }
        public DateTime? PremiumUntil { get; set; }
        public string? PurchaseToken { get; set; }

        public string? ResetToken { get; set; }
        public DateTime ResetTokenExpiration {  get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
        public Study? StudyState { get; set; }
        public ICollection<Word>? Words { get; set; }
    }
}
