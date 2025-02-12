using System.Linq.Expressions;

namespace DAL.Repositories.Contracts;

/// <summary>
/// Interface base para repositórios genéricos, definindo operações comuns de persistência.
/// </summary>
/// <typeparam name="T">O tipo da entidade manipulada pelo repositório.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Retorna todos os registros da entidade <typeparamref name="T"/>.
    /// </summary>
    /// <returns>Um <see cref="IQueryable{T}"/> contendo todos os registros.</returns>
    IQueryable<T> FindAll();

    /// <summary>
    /// Retorna os registros da entidade <typeparamref name="T"/> que atendem a uma condição específica.
    /// </summary>
    /// <param name="expression">Uma expressão lambda que define a condição.</param>
    /// <param name="tracking"></param>
    /// <returns>Um <see cref="IQueryable{T}"/> contendo os registros que atendem à condição.</returns>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool tracking = false);

    Task<T?> GetByIdAsync(int id, bool tracking = false);

    /// <summary>
    /// Verifica se existe pelo menos um registro que satisfaça uma condição específica.
    /// </summary>
    /// <param name="expression">Uma expressão lambda que define a condição.</param>
    /// <returns><c>true</c> se existir pelo menos um registro; caso contrário, <c>false</c>.</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Adiciona uma nova entidade ao repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser adicionada.</param>
    void Add(T entity);

    /// <summary>
    /// Atualiza uma entidade existente no repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada.</param>
    void Update(T entity);

    /// <summary>
    /// Remove uma entidade do repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser removida.</param>
    void Remove(T entity);

    /// <summary>
    /// Salva todas as alterações pendentes no repositório de forma assíncrona.
    /// </summary>
    /// <returns>Uma <see cref="Task"/> que representa a operação assíncrona.</returns>
    Task SaveChangesAsync();


}