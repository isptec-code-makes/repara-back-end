using Repara.DTO.Auth;

namespace Repara.Services.Contracts
{
    public interface IAuthService
    {
         Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string role = "Profissional");

         Task<RefreshTokenDto> RefreshTokenAsync(RefreshTokenDto request);

         Task<LoginResponseDto> LoginSocialUserAsync(SocialLoginRequest request);

         Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request);
    }
}