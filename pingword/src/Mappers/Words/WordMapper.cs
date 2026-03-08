using pingword.src.DTOs.Words;
using pingword.src.Models.Words;

namespace pingword.src.Mappers.Words
{
    public static class WordMapper
    {
        public static WordUpdateRequestDto ToDto(Word word)
        {
            return new WordUpdateRequestDto
            {
                Id = word.Id,
                Words = word.Words,
                Translation = word.Translation,
                Example = word.Example,
                IsDeleted = word.IsDeleted,
                UpdatedAt = word.UpdatedAt == DateTime.MinValue ? 0 : new DateTimeOffset(word.UpdatedAt).ToUnixTimeMilliseconds(),
                WordEnum = word.WordEnum,
                UserLevel = word.UserLevel,
            };
        }

        public static Word ToEntity(WordRequestDto wordRequestDto)
        {
            return new Word
            {
                Id = Guid.NewGuid(),
                Words = wordRequestDto.Words,
                Translation = wordRequestDto.Translation,
                Example = wordRequestDto.Example,
                IsDeleted =wordRequestDto.IsDeleted,
                UserId = wordRequestDto.UserId,
                WordEnum = wordRequestDto.WordEnum,
                UserLevel = wordRequestDto.UserLevel,
            };
        }
    }
}
