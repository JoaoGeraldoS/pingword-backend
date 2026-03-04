using pingword.src.Enums.Words;

namespace pingword.src.DTOs.Words
{
    public record WordIteractionDto
    {
        public WordInteractionEnum wordInteraction { get; set; }
    }
}
