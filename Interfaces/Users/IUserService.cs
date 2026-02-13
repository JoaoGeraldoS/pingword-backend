using pingword.DTOs.Users;

namespace pingword.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request);
    }
}
