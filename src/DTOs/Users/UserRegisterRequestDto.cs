namespace pingword.src.DTOs.Users
{
    public record UserRegisterRequestDto
    {
        public string Username { get; init; } = string.Empty;
        public string? Language { get; set; }
    }
}
