namespace Repara.Shared.Consts;

/// <summary>
/// Classe que define extensões de arquivo permitidas para diferentes tipos de documentos e imagens.
/// </summary>
public class FileExtensions
{
    /// <summary>
    /// Extensões de arquivo permitidas para imagens.
    /// </summary>
    public string[] Images { get; } = { ".jpg", ".png", ".jpeg" };

    /// <summary>
    /// Extensões de arquivo permitidas para documentos.
    /// </summary>
    public string[] Docs { get; } = { ".pdf", ".png", ".jpg" };

    /// <summary>
    /// Extensões de arquivo permitidas para importação de documentos.
    /// </summary>
    public string[] Import { get; } = { ".xlsx" };
}