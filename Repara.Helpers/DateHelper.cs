namespace Repara.Helpers;

/// <summary>
/// Classe utilitária para manipulação e conversão de formatos de data e hora.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Formato padrão para datas no formato "dia-mês-ano".
    /// Exemplo: "25-12-2024".
    /// </summary>
    private const string DateOnlyFormat = "dd-MM-yyyy";

    /// <summary>
    /// Formato padrão para datas e horas no formato "dia-mês-ano hora:minuto:segundo".
    /// Exemplo: "25-12-2024 14:30:00".
    /// </summary>
    private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

    /// <summary>
    /// Converte uma string no formato "dd-MM-yyyy" para um objeto <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="date">A string representando a data no formato "dd-MM-yyyy".</param>
    /// <returns>Um objeto <see cref="DateOnly"/> correspondente à data fornecida.</returns>
    /// <exception cref="FormatException">Lançada se o formato da string não corresponder ao formato esperado.</exception>
    public static DateOnly StringToDateOnly(string date)
    {
        return DateOnly.ParseExact(date, DateOnlyFormat);
    }

    /// <summary>
    /// Converte uma string no formato "dd-MM-yyyy HH:mm:ss" para um objeto <see cref="DateTime"/>.
    /// </summary>
    /// <param name="datetime">A string representando a data e hora no formato "dd-MM-yyyy HH:mm:ss".</param>
    /// <returns>Um objeto <see cref="DateTime"/> correspondente à data e hora fornecidas.</returns>
    /// <exception cref="FormatException">Lançada se o formato da string não corresponder ao formato esperado.</exception>
    public static DateTime StringToDateTime(string datetime)
    {
        return DateTime.ParseExact(datetime, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converte um objeto <see cref="DateOnly"/> em uma string no formato "dd-MM-yyyy".
    /// </summary>
    /// <param name="date">O objeto <see cref="DateOnly"/> a ser convertido.</param>
    /// <returns>Uma string representando a data no formato "dd-MM-yyyy".</returns>
    public static string DateOnlyToString(DateOnly date)
    {
        return date.ToString(DateOnlyFormat);
    }

    /// <summary>
    /// Converte um objeto <see cref="DateTime"/> em uma string no formato "dd-MM-yyyy".
    /// </summary>
    /// <param name="datetime">O objeto <see cref="DateTime"/> a ser convertido.</param>
    /// <returns>Uma string representando a data no formato "dd-MM-yyyy".</returns>
    public static string DateTimeToString(DateTime datetime)
    {
        return datetime.ToString(DateOnlyFormat);
    }
}
