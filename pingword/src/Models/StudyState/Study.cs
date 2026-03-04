using pingword.src.Enums.StudyState;
using pingword.src.Models.Users;

namespace pingword.src.Models.StudyState
{
    public class Study
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime LastInteraction { get; set; } = DateTime.UtcNow;
        public int DaysInteractedCount { get; set; } = 0;
        public Status Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public User? User { get; set; }


        public Study(string userId)
        {
            Id = Guid.NewGuid();
            LastInteraction = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
            Status = Status.ACTIVE;
            UserId = userId;
        }

        public void RegisterInteraction()
        {
            var today = DateTime.UtcNow.Date;

            if (Status == Status.INACTIVE) { 
                Status = Status.RETURNING;
                DaysInteractedCount = 1;
            }
            else if (Status == Status.RETURNING)
            {
                if (LastInteraction.Date < today)
                    DaysInteractedCount++;

                if (DaysInteractedCount >= 3)
                {
                    Status = Status.ACTIVE;
                    DaysInteractedCount = 0;
                }
            }

            LastInteraction = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
        }
        public void MarkAsInactive()
        {
            Status = Status.INACTIVE;
            LastUpdated = DateTime.UtcNow;
            DaysInteractedCount = 0;
        }
    }  
}
