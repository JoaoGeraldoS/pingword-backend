using pingword.src.Enums.Users;
using pingword.src.Enums.Words;

namespace pingword.src.DTOs.Words
{
    public record WordRequestDto
    {
        public string Words { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public string? UserId { get; set; }
        public WordEnum WordEnum { get; set; }
        public UserLevelEnum UserLevel { get; set; }
    }
}
