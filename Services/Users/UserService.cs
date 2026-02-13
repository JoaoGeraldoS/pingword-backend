using Microsoft.AspNetCore.Identity;
using pingword.DTOs.Users;
using pingword.Interfaces.Users;
using pingword.Models.Users;

namespace pingword.Services.Users
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request)
        {
            var existingUser = _userManager.FindByNameAsync(request.Username)
                ?? throw new InvalidOperationException("Username already exists.");

            var user = new User
            {
                UserName = request.Username,
                Language = request.Language,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) throw new InvalidOperationException("User Creation failed");
            
            return new UserRegisterResponseDto
            {
                Username = user.UserName,
                Language = user.Language!
            };
        }
    }
}
