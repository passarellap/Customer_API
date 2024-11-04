using AutoMapper;
using ControllerUtilities.Response;
using ServiceUtilities.Model;
using System.Collections.Generic;

namespace AnagraficaAPI.Mapping
{
    public class PagedDtoConverter<TSource, TDestination> : ITypeConverter<PagedViewModel<TSource>, PagedViewDto<TDestination>>
    {
        public PagedViewDto<TDestination> Convert(PagedViewModel<TSource> source, PagedViewDto<TDestination> destination, ResolutionContext context)
        {
            return new PagedViewDto<TDestination>()
            {
                CurrentPage = source.CurrentPage,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                OrderColumnsAvailables = source.OrderColumnsAvailables,
                Items = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.Items)
            };
        }
    }
}
