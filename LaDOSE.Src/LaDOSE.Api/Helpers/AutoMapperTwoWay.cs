using AutoMapper;
using AutoMapper.Configuration;

namespace LaDOSE.Api.Helpers
{
    public static class AutoMapperTwoWay
    {
        public static void CreateMapTwoWay<TSource, TDestination>(this IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<TSource, TDestination>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            mapper.CreateMap<TDestination, TSource>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
          
        }
    }
}