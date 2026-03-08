using pingword.src.DTOs.Users;

namespace pingword.src.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request);
        Task<LoginResponseDto?> LoginUser(LoginRequestDto request);
        Task<UserPerformaceDto> GetUserPerformanceAsync(string userId);
    }
}
