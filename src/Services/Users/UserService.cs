using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.src.DTOs.Users;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;

namespace pingword.src.Services.Users
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<User> userManager, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request)
        {
            _logger.LogInformation("Attempting to register user with username: {Username}", request.Username);

            var existingUser = _userManager.FindByNameAsync(request.Username)
                ?? throw new KeyNotFoundException("Username already exists.");

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
                Id = user.Id,
                Username = user.UserName,
                Language = user.Language!
            };
        }
        public async Task<UserPerformaceDto> GetUserPerformanceAsync(string userId)
        {
            _logger.LogInformation("Fetching performance data for user with ID: {UserId}", userId);

            var now = DateTime.UtcNow;
            var last7Days = now.AddDays(-7);
            var last30Days = now.AddDays(-30);

            var state = await _userRepository.GetStudyByUserId(userId);
            _logger.LogInformation("Study state for user {UserId}: {Status}, Last Interaction: {LastInteraction}", userId, state?.Status, state?.LastInteraction);

            var notification = _userRepository.GetUserNotificationsQuery(userId);
            
          

            var total = await notification.CountAsync();
            var last7DaysCount = await notification.Where(n => n.CreatedAt >= last7Days).CountAsync();
            var last30DaysCount = await notification.Where(n => n.CreatedAt >= last30Days).CountAsync();

            return new UserPerformaceDto
            {
                Status = state?.Status ?? Status.ACTIVE,
                LastInteractionAt = state?.LastInteraction,
                TotalInteractions = total,
                InteractionsLast7Days = last7DaysCount,
                InteractionsLast30Days = last30DaysCount
            };
        }
    }
}
