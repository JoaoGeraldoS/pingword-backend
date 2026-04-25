namespace pingword.src.DTOs.Users
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsFirstAccess { get; set; }
    }
}
