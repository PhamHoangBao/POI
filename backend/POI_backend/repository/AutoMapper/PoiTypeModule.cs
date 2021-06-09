using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class PoiTypeModule
    {
        public static void ConfigurePoiTypeModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Poitype, CreatePoiTypeViewModel>();
            mc.CreateMap<CreatePoiTypeViewModel, Poitype>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiTypeEnum.Available));
            mc.CreateMap<Poitype, UpdatePoiTypeViewModel>();
            mc.CreateMap<UpdatePoiTypeViewModel, Poitype>()
                .ForMember(des => des.Status, options => options.MapFrom(src => PoiTypeEnum.Available));
        }
    }
}
