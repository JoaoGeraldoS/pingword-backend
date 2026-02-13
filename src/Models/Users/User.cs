using Microsoft.AspNetCore.Identity;
using pingword.src.Models.Notifications;
using pingword.src.Models.StudyState;

namespace pingword.src.Models.Users
{
    public class User : IdentityUser
    {
        public string? Language {  get; set; }
        public DateTime TimeZone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Notification>? Notifications { get; set; }
        public Study? StudyState { get; set; }
    }
}
