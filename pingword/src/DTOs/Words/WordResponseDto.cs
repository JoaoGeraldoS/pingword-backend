namespace pingword.src.DTOs.Words
{
    public record WordResponseDto
    {
        public Guid Id { get; set; }
        public string Words { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;

        public string? UserId { get; set; }
    }
}
