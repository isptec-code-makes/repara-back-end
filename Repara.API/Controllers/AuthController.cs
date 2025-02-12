using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.DTO.Auth;
using Repara.Services.Contracts;

namespace Repara.API.Controllers;

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _authService.LoginUserAsync(request));
        }
        
        
        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest request)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _authService.LoginSocialUserAsync(request));
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            return Ok(await _authService.RegisterAsync(request));
        }
        
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        {
            return Ok(await _authService.RefreshTokenAsync(request));
        }
    }
