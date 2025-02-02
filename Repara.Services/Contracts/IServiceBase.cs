using Repara.DTO;

namespace Repara.Services.Contracts;

/// <summary>
/// Interface genérica para operações básicas de serviços.
/// </summary>
/// <typeparam name="T1">O tipo do DTO usado para retornar os dados.</typeparam>
/// <typeparam name="T2">O tipo do parâmetro usado para paginação ou filtragem.</typeparam>
/// <typeparam name="T3">O tipo do DTO usado para criação de novos registros.</typeparam>
/// <typeparam name="T4">O tipo do DTO usado para atualização de registros existentes.</typeparam>
public interface IServiceBase<T1, T2, T3, T4>
    where T1 : class
    where T3 : class
    where T4 : class
{
    /// <summary>
    /// Obtém uma lista paginada de objetos com base nos parâmetros fornecidos.
    /// </summary>
    /// <param name="parameters">Os parâmetros usados para filtrar e paginar os dados.</param>
    /// <returns>Uma lista paginada de objetos do tipo <typeparamref name="T1"/>.</returns>
    public PagedList<T1> GetAllPaged(T2 parameters);

    /// <summary>
    /// Obtém um objeto específico pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do objeto.</param>
    /// <returns>O objeto do tipo <typeparamref name="T1"/> correspondente ao ID fornecido.</returns>
    Task<T1?> GetByIdAsync(int id);

    /// <summary>
    /// Cria um novo objeto com base nos dados fornecidos.
    /// </summary>
    /// <param name="request">Os dados necessários para criar o novo objeto.</param>
    /// <returns>O objeto recém-criado do tipo <typeparamref name="T1"/> ou nulo se a criação falhar.</returns>
    Task<T1?> CreateAsync(T3 request);

    /// <summary>
    /// Atualiza um objeto existente com base no identificador e nos dados fornecidos.
    /// </summary>
    /// <param name="id">O identificador único do objeto a ser atualizado.</param>
    /// <param name="request">Os dados usados para atualizar o objeto.</param>
    /// <returns>O objeto atualizado do tipo <typeparamref name="T1"/>.</returns>
    Task<T1?> UpdateAsync(int id, T4 request);

    /// <summary>
    /// Exclui um objeto com base no seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do objeto a ser excluído.</param>
    Task DeleteAsync(int id);
}
