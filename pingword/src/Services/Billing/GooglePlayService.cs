
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PlayIntegrity.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pingword.src.Services.Billing
{
    public class GooglePlayService
    {
        private readonly AndroidPublisherService _publisherService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public GooglePlayService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;

            var json = Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("Variável de ambiente GOOGLE_SERVICE_ACCOUNT_JSON não encontrada!");
            }

            var credential = CredentialFactory
                .FromJson<ServiceAccountCredential>(json)
                .ToGoogleCredential()
                .CreateScoped(new[] {
                    AndroidPublisherService.Scope.Androidpublisher,
                    PlayIntegrityService.Scope.Playintegrity
                });

            _publisherService = new AndroidPublisherService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "PingWord"
            });
        }

        public async Task<bool> ValidatePremium(string purchaseToken, string userId)
        {
            try
            {
                Console.WriteLine($"DEBUG BILLING: Validando token: {purchaseToken.Substring(0, 10)}...");
        
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;
        
                // 1. TENTAR COMO SUBSCRIPTION PRIMEIRO
                try
                {
                    var result = await _publisherService.Purchases.Subscriptions
                        .Get("com.pingword.app", "premium-mensal", purchaseToken)
                        .ExecuteAsync();
        
                    return await UpdateUserPremium(user, result, purchaseToken);
                }
                catch (GoogleApiException subEx) when (subEx.HttpStatusCode == HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"DEBUG: Não é subscription. Tentando in-app... Error: {subEx.Message}");
                }
        
                // 2. TENTAR COMO IN-APP PURCHASE
                try
                {
                    var result = await _publisherService.Purchases.Products
                        .Get("com.pingword.app", "premium-mensal", purchaseToken)
                        .ExecuteAsync();
        
                    return await UpdateUserPremium(user, result, purchaseToken);
                }
                catch (GoogleApiException inAppEx)
                {
                    Console.WriteLine($"DEBUG: Falha também como in-app. Error: {inAppEx.Message}");
                    return false;
                }
        
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO GOOGLE API: {ex.Message}");
                return false;
            }
        }
        
        private async Task<bool> UpdateUserPremium(User user, dynamic purchaseResult, string purchaseToken)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
            DateTime? expiryDate = null;
            
            // Para Subscriptions
            if (purchaseResult.ExpiryTimeMillis.HasValue)
            {
                expiryDate = DateTimeOffset.FromUnixTimeMilliseconds(purchaseResult.ExpiryTimeMillis.Value).UtcDateTime;
            }
            // Para In-App Products
            else if (purchaseResult.ExpiryTimeMillis.HasValue || purchaseResult.PurchaseState == 0) // Purchased
            {
                // Para compras únicas, definir 1 ano ou conforme sua lógica
                expiryDate = DateTimeOffset.UtcNow.AddYears(1).UtcDateTime;
            }
        
            if (expiryDate.HasValue && expiryDate.Value > DateTime.UtcNow)
            {
                user.IsPremium = true;
                user.PurchaseToken = purchaseToken;
                user.PremiumUntil = expiryDate.Value;
        
                var update = await _userManager.UpdateAsync(user);
                return update.Succeeded;
            }
        
            return false;
        }

        public async Task<User> Restore(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not exists");

            return user;

        }

        public async Task<string> NewTokenJWT(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user); 

            
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("level", user.UserLevel.ToString()),
                new Claim("is_premium", "true"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            
            var token = _tokenService.GenerateAccessToken(authClaims);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        
    }
}
