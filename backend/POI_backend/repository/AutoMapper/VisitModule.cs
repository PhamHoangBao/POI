using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;


namespace POI.repository.AutoMapper
{
    public static class VisitModule
    {
        public static void ConfigureVisitModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Visit, CreateVisitViewModel>();
            mc.CreateMap<CreateVisitViewModel, Visit>()
                .ForMember(des => des.Status, options => options.MapFrom(src => VisitEnum.Available));
            mc.CreateMap<Visit, UpdateVisitViewModel>();
            mc.CreateMap<UpdateVisitViewModel, Visit>()
                .ForMember(des => des.Status, options => options.MapFrom(src => VisitEnum.Available));
        }
    }
}
