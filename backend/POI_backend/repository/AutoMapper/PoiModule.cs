using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class PoiModule
    {
        public static void ConfigurePoiModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Poi, CreatePoiViewModel>();
            mc.CreateMap<CreatePoiViewModel, Poi>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiEnum.Available));
            mc.CreateMap<Poi, UpdatePoiViewModel>();
            mc.CreateMap<UpdatePoiViewModel, Poi>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiEnum.Available));
        }
    }
}
