using pingword.src.Enums.Users;

namespace pingword.src.DTOs.Users
{
    public record UserProfileResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsPremium { get; set; }
        public string Language {  get; set; }
        public UserLevelEnum Level { get; set; }
    }
}
