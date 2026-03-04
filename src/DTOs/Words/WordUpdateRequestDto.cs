namespace pingword.src.DTOs.Words
{
    public record WordUpdateRequestDto
    {
        public Guid Id { get; set; }
        public string Words { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
