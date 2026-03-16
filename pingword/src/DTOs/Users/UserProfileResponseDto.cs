using pingword.src.Enums.Users;

namespace pingword.src.DTOs.Users
{
    public record UserProfileResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsPremium { get; set; }
        public string Language { get; set; } = string.Empty;
        public UserLevelEnum Level { get; set; }
    }
}
