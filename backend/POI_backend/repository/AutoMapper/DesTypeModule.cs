using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class DesTypeModule
    {
        public static void ConfigureDesTypeModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<DestinationType, CreateDesTypeViewModel>();
            mc.CreateMap<CreateDesTypeViewModel, DestinationType>()
                .ForMember(des => des.Status, options => options.MapFrom(src => DesTypeEnum.Available));
            mc.CreateMap<DestinationType, UpdateDesTypeViewModel>();
            mc.CreateMap<UpdateDesTypeViewModel, DestinationType>()
                .ForMember(des => des.Status, options => options.MapFrom(src => DesTypeEnum.Available));

        }
    }
}
