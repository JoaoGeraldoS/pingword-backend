using pingword.src.Enums.Users;

namespace pingword.src.DTOs.Users
{
    public record UserLevelRequest
    {
        public UserLevelEnum userLevel {  get; set; }

        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}
