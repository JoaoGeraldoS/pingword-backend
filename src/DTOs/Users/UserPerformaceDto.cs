using pingword.src.Enums.StudyState;

namespace pingword.src.DTOs.Users
{
    public record UserPerformaceDto
    {
        public Status Status { get; set; }
        public DateTime? LastInteractionAt { get; set; }
        public int TotalInteractions { get; set; }
        public int InteractionsLast7Days { get; set; }
        public int InteractionsLast30Days { get; set; }
    }
}
