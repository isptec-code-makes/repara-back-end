using System.Globalization;
using System.Linq.Expressions;

namespace Repara.Helpers
{
    public static class QueryableExtensions
    {

        /// <summary>
        /// Ordena dinamicamente uma fonte IQueryable por uma propriedade especificada.
        /// </summary>
        /// <typeparam name="T">O tipo dos elementos na coleção.</typeparam>
        /// <param name="source">A fonte IQueryable a ser ordenada.</param>
        /// <param name="chave">A chave da propriedade (possivelmente aninhada) pela qual ordenar.</param>
        /// <param name="descending">Indica se a ordenação deve ser descendente.</param>
        /// <param name="fallback">Nome da propriedade de fallback caso a chave especificada não seja encontrada.</param>
        /// <returns>Uma nova instância de IQueryable ordenada pela chave especificada.</returns>
        /// <exception cref="ArgumentException">Lançado quando a propriedade especificada não é encontrada.</exception>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> source, string chave, bool descending, string? fallback = "Id")
        {
            var propriedades = chave.Split('.');
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "p");
            Expression propertyAccess = parameter;

            try
            {
                foreach (var propriedade in propriedades)
                {
                    var nomePropriedade = propriedade.Length > 0
                        ? char.ToUpper(propriedade[0], CultureInfo.InvariantCulture) + propriedade.Substring(1)
                        : propriedade;

                    var property = propertyAccess.Type.GetProperty(nomePropriedade);
                    if (property == null)
                    {
                        throw new ArgumentException($"Propriedade '{nomePropriedade}' não encontrada no tipo '{propertyAccess.Type.Name}'.");
                    }

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            catch (ArgumentException ex)
            {
                if (fallback != null)
                {
                    var fallbackProperty = entityType.GetProperty(fallback);
                    if (fallbackProperty == null)
                    {
                        throw new ArgumentException($"Propriedade '{chave}' não encontrada e o fallback '{fallback}' não está disponível no tipo '{entityType.Name}'.", ex);
                    }

                    propertyAccess = Expression.MakeMemberAccess(parameter, fallbackProperty);
                }
                else
                {
                    throw new ArgumentException($"Propriedade '{chave}' não encontrada e nenhum fallback foi especificado.", ex);
                }
            }

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { entityType, propertyAccess.Type },
                source.Expression,
                Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
