using pingword.src.DTOs.FeedBacks;
using pingword.src.Models.FeedBacks;

namespace pingword.src.Mappers.FeedBacks
{
    public static class FeedBackMapper
    {
        public static FeedBackResponseDto ToDto(this FeedBack feedBack)
        {
            return new FeedBackResponseDto
            {
                Id = feedBack.Id,
                User = feedBack.User,
                Message = feedBack.Message,
                CreatedAt = feedBack.CreatedAt
            };
        }

        public static FeedBack ToEntity(this FeedBackRequestDto feedBackRequestDto)
        {
            return new FeedBack
            {
                User = feedBackRequestDto.User,
                Message = feedBackRequestDto.Message
           
            };
        }
    }
}
