using AutoMapper;
using ServiceUtilities.Model;
using ServiceUtilities.Paging;
using System.Collections.Generic;

namespace AnagraficaAPI.Domain.Mapping
{
    public class PagedModelConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedViewModel<TDestination>>
    {
        public PagedViewModel<TDestination> Convert(PagedList<TSource> source, PagedViewModel<TDestination> destination, ResolutionContext context)
        {
            return new PagedViewModel<TDestination>()
            {
                CurrentPage = source.CurrentPage,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                OrderColumnsAvailables = source.OrderColumnsAvailables,
                Items = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source)
            };
        }
    }
}
