namespace pingword.src.DTOs.Users
{
    public class UserRegisterResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
