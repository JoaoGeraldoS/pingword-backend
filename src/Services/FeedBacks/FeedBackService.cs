using FluentValidation;
using pingword.src.DTOs.FeedBacks;
using pingword.src.Interfaces.FeedBacks;
using pingword.src.Mappers.FeedBacks;

namespace pingword.src.Services.FeedBacks
{
    public class FeedBackService : IFeedBackService
    {
     
        private readonly IFeedBackRepository _repository;
        private readonly IValidator<FeedBackRequestDto> _validator;

        public FeedBackService(IFeedBackRepository repository, IValidator<FeedBackRequestDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task AddFeedBackAsync(FeedBackRequestDto feedBackRequestDto)
        {
            var validationResult = await _validator.ValidateAsync(feedBackRequestDto);
            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException("Falha! nome e mensagem são obrigatorias");
            }

            var feedBack = FeedBackMapper.ToEntity(feedBackRequestDto);
            await _repository.AddFeedBack(feedBack);
        }

        public async Task<List<FeedBackResponseDto>> GetAllFeedBacks()
        {
            var feedBacks = await _repository.GetAllFeedBacks();
            return feedBacks.Select(FeedBackMapper.ToDto).ToList();
        }

        public async Task<FeedBackResponseDto> GetFeedBackByIdAsync(Guid id)
        {
            var feedBack = await _repository.GetFeedBackById(id);
            if (feedBack == null)
            {
                throw new KeyNotFoundException("Feedback não encontrado");
            }

            return FeedBackMapper.ToDto(feedBack);
        }

        public async Task RemoveFeedBackAsync(Guid id)
        {
            var feedBack = await _repository.GetFeedBackById(id);
            if (feedBack == null)
            {
                throw new KeyNotFoundException("Feedback não encontrado");
            }

            await _repository.RemoveFeedBack(feedBack);
        }

    }
}
