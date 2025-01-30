namespace Repara.DTO;

/// <summary>
/// Classe que define os parâmetros de consulta para paginação, ordenação, pesquisa e filtros.
/// </summary>
public abstract class FilterParameters
{
    /// <summary>
    /// Propriedade que define o atributo utilizado como índice para ordenação.
    /// Valor padrão: "id".
    /// </summary>
    public string SortBy { get; set; } = "id";

    /// <summary>
    /// Define a ordem dos dados: crescente ou decrescente.
    /// Valor padrão: false (ordem crescente).
    /// </summary>
    public bool IsDecsending { get; set; } = true;

    /// <summary>
    /// Define uma palavra-chave para realizar a pesquisa nos dados.
    /// Valor padrão: string vazia.
    /// </summary>
    public string Search { get; set; } = string.Empty;

    /// <summary>
    /// Número máximo permitido de itens por página.
    /// Constante definida com valor de 100.
    /// </summary>
    private const int MaxPageSize = 100;
    
    /// <summary>
    /// Define se será usando o MaxPageSize
    /// </summary>
    public bool UseMaxPageSize { get; set; } = true;

    /// <summary>
    /// Número da página a ser consultada.
    /// Valor padrão: 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamanho padrão da página, com limite máximo de itens permitido.
    /// Valor padrão: 50.
    /// </summary>
    private int _pageSize = 50;

    /// <summary>
    /// Propriedade que define o número de itens por página.
    /// Se o valor definido exceder o limite máximo (MaxPageSize), será ajustado para o máximo permitido.
    /// </summary>
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            // Garante que o tamanho da página não exceda o máximo permitido.
            if (!UseMaxPageSize)
            {
                _pageSize = int.MaxValue;
            }
            else
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
    
    /// <summary>
    /// Propriedade que permite filtrar os dados com base em uma data específica de criação.
    /// Tipo: DateOnly.
    /// </summary>
    public DateOnly? CreatedOn { get; set; }
    
    /// <summary>
    /// Inicio do intervalo da cobranca.
    /// Usado para filtrar cobranças por intervalo de tempo.
    /// </summary>
    public string? DataInicio { get; set; }
    
    /// <summary>
    /// Fim do intervalo da cobranca
    /// Usado para filtrar cobranças por intervalo de tempo.
    /// </summary>
    public string? DataFim { get; set; }
}
