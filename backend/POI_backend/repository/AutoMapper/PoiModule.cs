using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;
using NetTopologySuite.Geometries;


namespace POI.repository.AutoMapper
{
    public static class PoiModule
    {
        public static void ConfigurePoiModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Poi, CreatePoiViewModel>();
            mc.CreateMap<CreatePoiViewModel, Poi>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiEnum.Available))
                .ForMember(des => des.Location, options => options.MapFrom(src => new Point(src.Location.Longtitude, src.Location.Latitude)));
            mc.CreateMap<Poi, UpdatePoiViewModel>();
            mc.CreateMap<UpdatePoiViewModel, Poi>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiEnum.Available))
                .ForMember(des => des.Location, options => options.MapFrom(src => new Point(src.Location.Longtitude, src.Location.Latitude)));
            mc.CreateMap<Poi, ResponsePoiViewModel>()
                 .ForMember(des => des.Location,
                            options => options.MapFrom(src => new MyPoint(src.Location.Coordinate.Y, src.Location.Coordinate.X)));

        }
    }
}
