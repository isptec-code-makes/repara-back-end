using System.ComponentModel.DataAnnotations;
using Repara.Model.Enum;

namespace Repara.DTO.Auth;

public class SocialLoginRequest
{
    //[MinLength(Consts.UsernameMinLength, ErrorMessage = Consts.UsernameLengthValidationError)]
    public string? Email { get; set; }

    [Required] 
    public LoginProvider Provider { get; set; }

    [Required] 
    public string? AccessToken { get; set; }
}