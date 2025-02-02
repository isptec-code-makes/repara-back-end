using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repara.DTO.Auth;
using Repara.Helpers;
using Repara.Model;
using Repara.Model.Enum;
using Repara.Services.Contracts;
using Repara.Shared.Consts;
using Repara.Shared.Exceptions;
using Google.Apis.Auth;

namespace Repara.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly JwtHandler _jwtHandler;
        private readonly SocialLoginConfiguration _socialLoginConfiguration;

        public AuthService(UserManager<User> userManager, IOptions<JwtConfiguration> jwtConfiguration, JwtHandler jwtHandler,  IOptions<SocialLoginConfiguration> socialLoginConfig)
        {
            _userManager = userManager;
            _jwtConfiguration = jwtConfiguration.Value;
            _jwtHandler = jwtHandler;
            _socialLoginConfiguration = socialLoginConfig.Value;
        }   

        public async Task<LoginResponseDto> LoginSocialUserAsync(SocialLoginRequest request)
        {
            await ValidateSocialToken(request);

            var user = await _userManager.FindByEmailAsync(request.Email!) ?? await _userManager.FindByNameAsync(request.Email) ?? await RegisterSocialUser(request);

            var token = await _jwtHandler.CreateToken(user);

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = _jwtHandler.GenerateRefreshToken()
            };
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username!) ?? await _userManager.FindByEmailAsync(request.Username!);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password!))
            {
                throw new UnauthorizedException("Usuário ou senha inválidos");
            }
        
            if (user.Provider != LoginProvider.Local)
            {
                throw new UnauthorizedException($"Usuario registrado via {user.Provider.ToString()} e não pode ser fazer login via {LoginProvider.Local}.");
            }

            var token = await _jwtHandler.CreateToken(user);
            var refreshToken = _jwtHandler.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTimeOffset.Now.AddDays(14);

            await _userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        // método para atualizar o token de acesso e o refresh token

        public async Task<RefreshTokenDto> RefreshTokenAsync(RefreshTokenDto request)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                throw new UnauthorizedException("Token de acesso ou token de atualização inválido");
            }

            var username = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new UnauthorizedException("Token de acesso ou token de atualização inválido");
            }

            var newAccessToken = await _jwtHandler.CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new RefreshTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        // método pra registrar um usuário com o provider local
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, string role = "Profissional")
        {

             var userByEmail = await _userManager.FindByEmailAsync(request.Email!);

            var userByUsername = await _userManager.FindByNameAsync(request.Telefone!);
            
        
            if (userByEmail is not null || userByUsername is not null)
            {
                throw new ConflictRequestException(ExceptionConstants.UsuarioExists);
            }

            User user = new()
            {
                Email = request.Email,
                UserName = request.Telefone,
                Provider = LoginProvider.Local,
                PhoneNumber = request.TelefoneCompleto
            };

            var result = await _userManager.CreateAsync(user, request.Password);
        

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"errors: {GetErrorsText(result.Errors)}");
            }
            
            await _userManager.AddToRoleAsync(user, role);

            return new RegisterResponseDto(){
                UserId = user.Id
            };
        }


        private static string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join("; ", errors.Select(error => error.Description).ToArray());
        }

         private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

         private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

         private async Task ValidateSocialToken(SocialLoginRequest request)
        {
            var _ = request.Provider switch
            {
                //LoginProvider.Facebook => await ValidateFacebookToken(request),
                LoginProvider.Google => await ValidateGoogleToken(request),
                _ => throw new UnauthorizedException("Invalid provider")
            };
        }

        /*
        private async Task<bool> ValidateFacebookToken(SocialLoginRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var appAccessTokenResponse = await httpClient.GetFromJsonAsync<FacebookAppAccessTokenResponse>($"https://graph.facebook.com/oauth/access_token?client_id={_socialLoginConfiguration.Facebook!.ClientId!}&client_secret={_socialLoginConfiguration.Facebook!.ClientSecret!}&grant_type=client_credentials");
            var response =
                await httpClient.GetFromJsonAsync<FacebookTokenValidationResult>(
                    $"https://graph.facebook.com/debug_token?input_token={request.AccessToken}&access_token={appAccessTokenResponse!.AccessToken}");

            if (response is null || !response.Data.IsValid)
            {
                throw new Exception($"{request.Provider} access token is not valid.");
            }
        
            return true;
        }
        */

        private async Task<bool> ValidateGoogleToken(SocialLoginRequest request)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _socialLoginConfiguration.Google!.TokenAudience! }
                };
                await GoogleJsonWebSignature.ValidateAsync(request.AccessToken, settings);
                
            }
            catch (InvalidJwtException)
            {
                throw new UnauthorizedException($"Provedor: {request.Provider}, o token de acesso não é válido .");
            }
        
            return true;
        }

        private async Task<User> RegisterSocialUser(SocialLoginRequest request)
        {
            var user = new User()
            {
                Email = request.Email,
                UserName = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Provider = request.Provider!
            };
                
            var result = await _userManager.CreateAsync(user, $"Pass!1{Guid.NewGuid().ToString()}");
            
            if(!result.Succeeded)
            {
                throw new Exception($"Unable to register user {request.Email}, errors: {GetErrorsText(result.Errors)}");
            }

            await _userManager.AddToRoleAsync(user, "Cliente");
        
            return user;
        }
    }
}
