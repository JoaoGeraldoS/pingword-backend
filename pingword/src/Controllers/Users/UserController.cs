using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Users;
using pingword.src.Interfaces.Users;
using pingword.src.Models.Users;
using Serilog;
using System.Security.Claims;

namespace pingword.src.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

       
        public UserController(IUserService userService)
        {
            _userService = userService; 
        }

        [HttpPost]
        public async Task<ActionResult<UserRegisterResponseDto>> RegisterUser([FromBody] UserRegisterRequestDto request)
        {
            Log.Information("Attempting to register user with email: {Email}", request.Email);
            var response = await _userService.RegisterUserAsync(request);
            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> LoginUser([FromBody] LoginRequestDto request)
        {
            Log.Information("Attempting to log in user with email: {Email}", request.Email);
            var response = await _userService.LoginUser(request);
            
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto tokenDto)
        {
            var refresh = await _userService.RefreshToken(tokenDto);
            return Ok(refresh);
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.Revoke(userId!);
            if (!result) return BadRequest("User not exists");

            return Ok();
        }



        [Authorize]
        [HttpGet("performance")]
        public async Task<ActionResult<UserPerformaceDto>> GetUserPerformance()
        {
            Log.Information("Getting user performance for user: {User}", User.Identity?.Name);
            var userClaym = User.FindFirst(ClaimTypes.NameIdentifier);
            Log.Information("User claim: {UserClaim}", userClaym?.Value);

            if (userClaym == null) { return Unauthorized(); }


            var response = await _userService.GetUserPerformanceAsync(userClaym.Value.ToString());
            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileResponseDto>> GetProfileAsync()
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userClaim == null) { return Unauthorized(); }

            var response = await _userService.GetProfileAsync(userClaim.Value.ToString());
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Unauthorized(); }

            var deleted = await _userService.DeleteAccountAsync(userId);
            return Ok(deleted);
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var sent = await _userService.ForgotPasswordAsync(request);
            Log.Information($"Forgot password: {sent}");
            return sent ? Ok() : StatusCode(500, "Erro ao enviar email");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            await _userService.ResetPassword(request);
            return Ok("Senha alterada com sucesso!");
        }


        [Authorize]
        [HttpPatch("user-level")]
        public async Task<IActionResult> UpdateUserLevel([FromBody] UserLevelRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Unauthorized(); }

            var result = await _userService.UpdateLevelUser(userId, request);

            return Ok(result);
        }
    }
}
