using pingword.src.Enums.Users;

namespace pingword.src.DTOs.Users
{
    public class UserRegisterResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public UserLevelEnum UserLevel { get; set; }
    }
}
