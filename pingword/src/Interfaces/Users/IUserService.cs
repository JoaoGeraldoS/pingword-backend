using pingword.src.DTOs.Users;

namespace pingword.src.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request);
        Task<LoginResponseDto?> LoginUser(LoginRequestDto request);
        Task<UserPerformaceDto> GetUserPerformanceAsync(string userId);
        Task<UserProfileResponseDto> GetProfileAsync(string userId);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto request);
        Task<bool> ResetPassword(ResetPasswordDto request);
        Task<bool> DeleteAccountAsync(string userID);

        Task<RefreshTokenDto> UpdateLevelUser(string userID, UserLevelRequest request);
        Task<RefreshTokenDto> RefreshToken(RefreshTokenDto tokenDto);
        Task<bool> Revoke(string userId);
    }
}
