using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.ViewModels;
using AutoMapper;

namespace POI.repository.AutoMapper
{
    public static class CustomModule
    {
        private class PageListConverter<TSource, TDestination>
                : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
        {
            public PagedList<TDestination> Convert(
                PagedList<TSource> source,
                PagedList<TDestination> destination,
                ResolutionContext context) =>
                new PagedList<TDestination>(
                    context.Mapper.Map<List<TSource>, List<TDestination>>(source),
                    source.TotalCount,
                    source.CurrentPageIndex,
                    source.PageSize);
        }

        public static void ConfigureCustomModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PageListConverter<,>));
        }
    }
}
