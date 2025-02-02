using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Repara.Shared.DataAnnotations;

public class DateFormatAttribute : ValidationAttribute
{
    private readonly string _formato;

    public DateFormatAttribute(string formato)
    {
        _formato = formato;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        DateTime data;
        bool valido = DateTime.TryParseExact(value.ToString(), _formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out data);

        if (!valido)
        {
            return new ValidationResult($"A data deve estar no formato {_formato}.");
        }

        return ValidationResult.Success;
    }
}