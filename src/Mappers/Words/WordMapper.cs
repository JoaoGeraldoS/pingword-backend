using pingword.src.DTOs.Words;
using pingword.src.Models.Words;

namespace pingword.src.Mappers.Words
{
    public static class WordMapper
    {
        public static WordResponseDto ToDto(Word word)
        {
            return new WordResponseDto
            {
                Id = word.Id,
                Words = word.Words,
                Translation = word.Translation,
                Example = word.Example,
                UserId = word.UserId
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
                UserId = wordRequestDto.UserId
            };
        }
    }
}
