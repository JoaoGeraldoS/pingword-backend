namespace pingword.src.DTOs.Users
{
    public record RefreshTokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}
