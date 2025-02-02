using System.ComponentModel.DataAnnotations;

namespace Repara.DTO.Auth;

// TODO: Configurar os dataannotations para validar o tamanho dos campos
public class LoginRequestDto
{
    //[MinLength(Consts.UsernameMinLength, ErrorMessage = Consts.UsernameLengthValidationError)]
    public string? Username { get; set; }

    //[RegularExpression(Consts.PasswordRegex, ErrorMessage = Consts.PasswordValidationError)]
    public string? Password { get; set; }
}
