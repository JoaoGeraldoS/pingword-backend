using pingword.src.DTOs.Words;
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

        public async Task<List<WordResponseDto>> GetWordsAsync(string userID)
        {
            var user = await _repository.GetWordByUser(userID);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var wors = await _repository.GetAllWords(user.UserId!);
            return wors.Select(WordMapper.ToDto).ToList();  
        }

        public async Task SyncWords(List<WordUpdateRequestDto> words, string userId)
        {
            foreach (var word in words)
            {
                var dbWord = await _repository.GetById(userId, word.Id);
               

                if (dbWord != null)
                {
                    if (word.IsDeleted)
                    {
                        dbWord.IsDeletd = true;
                        dbWord.UpdatedAt = word.UpdatedAt;
                        await _repository.DeleteWord(dbWord);
                    }
                    else
                    {
                        dbWord.Words = word.Words;
                        dbWord.Translation = word.Translation;
                        dbWord.Example = word.Example;
                        dbWord.UpdatedAt = word.UpdatedAt; 
                    }

                }
       
                else if (!word.IsDeleted)
                {
                    
                    var newWord = new Word
                    {
                        Id = word.Id,
                        UserId = userId,
                        Words = word.Words,
                        Translation = word.Translation,
                        Example = word.Example,
                        UpdatedAt = word.UpdatedAt
                    };

                    await _repository.AddWord(newWord);
                }
            }
            await _repository.SaveChangesAsync();
        }

        public async Task WordInteractionUpdate(string userId, Guid id, WordInteractionEnum iteraction)
        {
            var word = await _repository.GetById(userId, id);
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
