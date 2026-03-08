using pingword.src.DTOs.Words;
using pingword.src.Enums.Users;
using pingword.src.Enums.Words;
using pingword.src.Interfaces.Words;
using pingword.src.Mappers.Words;
using pingword.src.Models.Words;

namespace pingword.src.Services.Words
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _repository;

        public WordService(IWordRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WordUpdateRequestDto>> GetWordsAsync(string userID, UserLevelEnum userLevel)
        {
            var user = await _repository.GetWordByUser(userID);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var wors = await _repository.GetAllWords(user.UserId!, userLevel);
            return wors.Select(WordMapper.ToDto).ToList();  
        }

        public async Task SyncWords(List<WordUpdateRequestDto> words, string userId)
        {
            foreach (var word in words)
            {

                if (word.WordEnum != WordEnum.USER) continue;

                var dbWord = await _repository.GetByIdInternal(word.Id);
                var updatedAt = DateTimeOffset.FromUnixTimeMilliseconds(word.UpdatedAt).UtcDateTime;


                if (dbWord != null)
                {

                    if (dbWord.UserId != userId) continue;

                    if (updatedAt > dbWord.UpdatedAt)
                    {

                        if (word.IsDeleted)
                        {
                            dbWord.IsDeleted = true;
                            dbWord.UpdatedAt = updatedAt;
                            await _repository.DeleteWord(dbWord);
                        }
                        else
                        {


                            dbWord.Words = word.Words;
                            dbWord.Translation = word.Translation;
                            dbWord.Example = word.Example;
                            dbWord.UpdatedAt = updatedAt;

                            await _repository.UpdateWrod(dbWord);
                        }
                    }
                }
                else if (!word.IsDeleted)
                {
                   
                    var existingByText = await _repository.GetByText(userId, word.Words, word.Translation);

                    if (existingByText == null)
                    {
                        var newWord = new Word
                        {
                            Id = word.Id, 
                            UserId = userId,
                            Words = word.Words,
                            Translation = word.Translation,
                            Example = word.Example,
                            UpdatedAt = updatedAt,
                            IsDeleted = word.IsDeleted,
                            UserLevel = word.UserLevel,
                            WordEnum = word.WordEnum,
                        };
                        await _repository.AddWord(newWord);
                    }
                
                }
            }
            await _repository.SaveChangesAsync();
        }

        public async Task WordInteractionUpdate(string userId, Guid id, WordInteractionEnum iteraction)
        {
            var word = await _repository.GetByIdWithUser(userId, id);
            if (word == null)
            {
                throw new KeyNotFoundException("Word not found");
            }

            word.InteractionEnum = iteraction;
            word.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateWordInteractio(word);

            await _repository.SaveChangesAsync();
        }
    }
}
