using Microsoft.AspNetCore.Identity;
using pingword.Models.Notifications;
using pingword.Models.StudyState;

namespace pingword.Models.Users
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
