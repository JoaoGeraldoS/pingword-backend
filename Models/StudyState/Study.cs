using pingword.Enums.StudyState;
using pingword.Models.Users;

namespace pingword.Models.StudyState
{
    public class Study
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime LastInteraction { get; set; } = DateTime.UtcNow;

        public Status Status { get; set; }
        public DateTime LastUpdated { get; set; }

        public User? User { get; set; }

    }
}
