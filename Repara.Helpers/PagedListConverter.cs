using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Repara.DTO;

namespace Repara.Helpers
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        private readonly IMapper _mapper;

        public PagedListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var items = _mapper.Map<List<TDestination>>(source);

            // Retorna uma nova PagedList com os itens mapeados
            return new PagedList<TDestination>(items, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
