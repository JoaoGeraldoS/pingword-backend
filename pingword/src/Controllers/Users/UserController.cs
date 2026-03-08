using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pingword.src.DTOs.Users;
using pingword.src.Interfaces.Users;
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
    }
}
