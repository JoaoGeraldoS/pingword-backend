using pingword.src.Enums.Words;
using pingword.src.Models.Users;

namespace pingword.src.Models.Words
{
    public class Word
    {
        public Guid Id { get; set; }
        public string Words { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public WordInteractionEnum InteractionEnum { get; set; }
        public bool IsDeletd { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }


        public User? Users { get; set; }
    }
}
