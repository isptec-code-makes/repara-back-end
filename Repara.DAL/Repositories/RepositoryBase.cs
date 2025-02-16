using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repara.DAL.Repositories;


/// <summary>
/// Classe base abstrata para a implementação de repositórios genéricos.
/// Fornece métodos comuns para manipulação de entidades no contexto de banco de dados.
/// </summary>
/// <typeparam name="T">O tipo da entidade manipulada pelo repositório.</typeparam>
/// /// <typeparam name="T2">Filtro para listas paginadas.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    /// <summary>
    /// Contexto do banco de dados usado para acessar e manipular os dados.
    /// </summary>
    protected readonly AppDbContext _appDbContext;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RepositoryBase{T}"/> com o contexto de banco de dados fornecido.
    /// </summary>
    /// <param name="appDbContext">O contexto do banco de dados.</param>
    protected RepositoryBase(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    /// <inheritdoc/>
    public IQueryable<T> FindAll()
    {
        return _appDbContext.Set<T>();
    }

    protected DbSet<T> Entity()
    {
        return _appDbContext.Set<T>();
    }

    /// <inheritdoc/>
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool tracking = true)
    {
        if (tracking)
        {
            return _appDbContext.Set<T>().Where(expression).AsTracking();
        }
        else
        {
            return _appDbContext.Set<T>().Where(expression).AsNoTracking();
        }
    }

    public async Task<T?> GetByIdAsync(int id, bool tracking = false)
    {
        if (tracking)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }
        else
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        => await _appDbContext.Set<T>().AnyAsync(expression);

    /// <inheritdoc/>
    public void Add(T entity)
        => _appDbContext.Set<T>().Add(entity);

    /// <inheritdoc/>
    public void Update(T entity)
        => _appDbContext.Set<T>().Update(entity);

    /// <inheritdoc/>
    public void Remove(T entity)
        => _appDbContext.Remove(entity);

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
        => await _appDbContext.SaveChangesAsync();

}
