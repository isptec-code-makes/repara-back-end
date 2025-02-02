using System.ComponentModel.DataAnnotations;

namespace Repara.DTO.Auth;

public class RegisterRequestDto
{
    // núemero de telefone sem o código do páis, para logi
    //[MinLength(Consts.UsernameMinLength, ErrorMessage = Consts.UsernameLengthValidationError)]
    public string? Telefone { get; set; }

    // número de telefone com o código do país
    public string? TelefoneCompleto { get; set; }

    //[EmailAddress(ErrorMessage = Consts.EmailValidationError)]
    public string? Email { get; set; }

    //[RegularExpression(Consts.PasswordRegex, ErrorMessage = Consts.PasswordValidationError)]
    public string Password { get; set; }
}
