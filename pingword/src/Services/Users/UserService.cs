using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pingword.src.DTOs.Users;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace pingword.src.Services.Users
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IValidator<UserRegisterRequestDto> _validator;
        private readonly ITokenService _tokenService;
        public UserService(
            UserManager<User> userManager, IUserRepository userRepository,
            ILogger<UserService> logger, IValidator<UserRegisterRequestDto> validator,
            ITokenService token)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
            _validator = validator;
            _tokenService = token;
        }

        public async Task<UserRegisterResponseDto> RegisterUserAsync(UserRegisterRequestDto request)
        {
            _logger.LogInformation("Attempting to register user with username: {Username}", request.Username);


            var validateUser = await _validator.ValidateAsync(request);
            if (!validateUser.IsValid)
            {
                throw new Exception(string.Join("; ", validateUser.Errors.Select(e => e.ErrorMessage)));
            }

            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null) 
            {
                throw new InvalidOperationException("User exists");
            }

            var user = new User
            {
                Name = request.Username,
                Email = request.Email,
                Language = request.Language,
                UserName = Guid.NewGuid().ToString(),
                UserLevel = request.UserLevel,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Falha na criação do Identity: {Errors}", errorMessages);
                throw new InvalidOperationException("User Creation failed");
            }
            return new UserRegisterResponseDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                Language = user.Language!,
                UserLevel = user.UserLevel,
            };
        }
       
        public async Task<LoginResponseDto?> LoginUser(LoginRequestDto request)
        {
            var getUser = await _userManager.FindByEmailAsync(request.Email);
            if (getUser == null || !await _userManager.CheckPasswordAsync(getUser, request.Password))
                return null;
            
            var userRoles = await _userManager.GetRolesAsync(getUser);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, getUser.Name!),
                new Claim(ClaimTypes.Email, getUser.Email!),
                new Claim("level", getUser.UserLevel.ToString()),
                new Claim(ClaimTypes.NameIdentifier, getUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = _tokenService.GenerateAccessToken(authClaims);

            await _userManager.UpdateAsync(getUser);
            return new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

        }

        public async Task<UserPerformaceDto> GetUserPerformanceAsync(string userId)
        {
            _logger.LogInformation("Fetching performance data for user with ID: {UserId}", userId);

            var now = DateTime.UtcNow;
            var last7Days = now.AddDays(-7).Date;
            var last30Days = now.AddDays(-30).Date;

            var state = await _userRepository.GetStudyByUserId(userId);
            _logger.LogInformation("Study state for user {UserId}: {Status}, Last Interaction: {LastInteraction}", userId, state?.Status, state?.LastInteraction);

            var notification = _userRepository.GetUserNotificationsQuery(userId);

           

            var total = await notification.CountAsync();
            var last7DaysCount = await notification
                .Where(n => n.CreatedAt >= last7Days)
                .Select(n => n.CreatedAt.Date)
                .Distinct().CountAsync();

            var last30DaysCount = await notification
                .Where(n => n.CreatedAt >= last30Days)
                .Select(n => n.CreatedAt.Date)
                .Distinct().CountAsync();

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
