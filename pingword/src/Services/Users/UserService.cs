using FluentValidation;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using pingword.src.DTOs.Users;
using pingword.src.Enums.StudyState;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace pingword.src.Services.Users
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IValidator<UserRegisterRequestDto> _validator;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        
        public UserService(
            UserManager<User> userManager, IUserRepository userRepository,
            ILogger<UserService> logger, IValidator<UserRegisterRequestDto> validator,
            ITokenService token, IConfiguration config)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
            _validator = validator;
            _tokenService = token;
            _config = config;
            
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
                new Claim("is_premium", getUser.IsPremium.ToString().ToLower()),
                new Claim(ClaimTypes.NameIdentifier, getUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            getUser.RefreshToken = refreshToken;
            getUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(getUser);
            return new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };

        }

        public async Task<bool> Revoke(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.RefreshToken = null;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<RefreshTokenDto> RefreshToken(RefreshTokenDto tokenDto)
        {

            var responseInvalide = new RefreshTokenDto
            {
                AccessToken = "AccssToken invalido",
                RefreshToken = "Refresh invalido",
            };

            if (string.IsNullOrEmpty(tokenDto.AccessToken) || string.IsNullOrEmpty(tokenDto.RefreshToken))
                return responseInvalide;

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            if (principal == null) return responseInvalide;

          
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return responseInvalide;
            }

           
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
            await _userManager.UpdateAsync(user);

            return new RefreshTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
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

        public async Task<UserProfileResponseDto> GetProfileAsync(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) { throw new KeyNotFoundException("User not exists"); }


            return new UserProfileResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                IsPremium = user.IsPremium,
                Language = user.Language!,
                Level = user.UserLevel
            };
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            
            if (user == null) return false;

            var token = new Random().Next(100000, 999999).ToString();
            user.ResetToken = token;
            user.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(15);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return false;
            }

            var sent = await SendEmailApi(user.Email!, token);
            return sent;
        }


        public async Task<bool> ResetPassword(ResetPasswordDto request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.ResetToken == request.Token &&
                u.ResetTokenExpiration > DateTime.UtcNow);
            if (user == null) return false;

            var NewPasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword!);
            user.PasswordHash = NewPasswordHash;

            await _userManager.UpdateSecurityStampAsync(user);

            user.ResetToken = null;
            user.ResetTokenExpiration = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;

        }


        private async Task<bool> SendEmailApi(string email, string token)
        {
            try
            {
                using var client = new HttpClient();
                var apiKey = _config["Brevo:ApiKey"];

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("api-key", apiKey);

                var payload = new
                {
                    // Mantenha o seu e-mail validado aqui até você comprar um domínio
                    sender = new { name = "PingWord App", email = "pinglabs.dev@gmail.com" },
                    to = new[] { new { email = email } },
                    subject = "Recuperação de Senha - PingWord",
                    htmlContent = $@"
                <div style='font-family: sans-serif; max-width: 600px; margin: auto;'>
                    <h2>Seu código de acesso</h2>
                    <p>Use o código abaixo para redefinir sua senha no app:</p>
                    <h1 style='color: #00E5FF;'>{token}</h1>
                    <p>Se não foi você, ignore este e-mail.</p>
                </div>"
                };

                var response = await client.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", payload);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro crítico ao enviar: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAccountAsync(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) return false;

            
            await _userRepository.DeleteRange(user.Id);

            var userDeleted = await _userManager.DeleteAsync(user);
            return userDeleted.Succeeded;

        }

        public async Task<RefreshTokenDto> UpdateLevelUser(string userID, UserLevelRequest request)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) throw new KeyNotFoundException("User not exists");

            user.UserLevel = request.userLevel;
            
            var response = await _userManager.UpdateAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("level", user.UserLevel.ToString()),
                new Claim("is_premium", user.IsPremium.ToString().ToLower()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var newAccessToken = _tokenService.GenerateAccessToken(authClaims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();


            return new RefreshTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }
    }
}
