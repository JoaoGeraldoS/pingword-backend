using pingword.src.DTOs.Users;

namespace pingword.src.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request);
    }
}
