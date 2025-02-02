using Microsoft.AspNetCore.Identity;
using Repara.Model.Enum;

namespace Repara.Model;

public class User : IdentityUser
{
    public LoginProvider Provider { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}