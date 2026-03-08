using pingword.src.Enums.Users;

namespace pingword.src.DTOs.Users
{
    public record UserRegisterRequestDto
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string? Language { get; set; }
        public UserLevelEnum UserLevel { get; set; }
    }
}
